using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : GlobalController
{
    #region Public Fields

    [Header("Arrow")]
    public GameObject Arrow;
    public GameObject ArrowRotationCenter;

    #endregion

    #region Unity Functions

    private void Start()
    {
         Init();
        _distArrowRayPoint = Arrow.transform.position - RayPoint.position;
    }

    private void Update()
    {
        _distArrowRayPoint = Arrow.transform.position - RayPoint.position;

        if (GameManager.Instance.GameState == GameState.RACING && PlayerState == PlayerState.ALIVE && _movementInput != Vector2.zero)
        {
            Arrow.SetActive(true);

            _wantedDirection = new Vector3(_movementInput.x, _yComponentWantedDirection, _movementInput.y);

            if (_isGrounded)
            {
                Vector3 cross = Vector3.Cross(transform.forward, _wantedDirection);
                float carSignRotation = Mathf.Sign(cross.y);

                if (Mathf.Abs(Mathf.Acos(Vector3.Dot(transform.forward.normalized, _wantedDirection.normalized))) > Mathf.Deg2Rad * 10f)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, carSignRotation * TurnStrength * Time.deltaTime, 0f));
                    _canMove = false;
                }
                else
                {
                    transform.forward = _wantedDirection;
                    _canMove = true;
                }

                if (Mathf.Abs(Mathf.Acos(Vector3.Dot(ArrowRotationCenter.transform.forward.normalized, _wantedDirection.normalized))) > Mathf.Deg2Rad * 1f)
                    ArrowRotationCenter.transform.forward = _wantedDirection;
            }
        }
        else if (GameManager.Instance.GameState != GameState.RACING)
        {
            Arrow.SetActive(false);
            _canMove = false;
            SphereReference.transform.position = _lastRebornPosition;
            SphereRB.velocity = Vector3.zero;
        }
        else
        {
            Arrow.SetActive(false);
            _canMove = false;
        }

        transform.position = SphereReference.transform.position + _carAltitudeOffset;
    }

    private void FixedUpdate()
    {
        UpdateGraphics();
    }
    
    #endregion

    #region Related to in-game actions

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (BoostsContainer.transform.childCount == 0)
                return;

            if (BoostsContainer.transform.GetComponentInChildren<Booster>())
            {
                BoostsContainer.transform.GetChild(0).GetComponent<Booster>().Boost(SphereRB, gameObject);
            }

            ProfileUI.UseBoost();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (AttacksContainer.transform.childCount == 0)
                return;

            if (AttacksContainer.transform.GetComponentInChildren<Offensive>())
            {
                AttacksContainer.transform.GetChild(0).GetComponent<Offensive>().Shoot();
            }

            ProfileUI.UseWeapon();
        }
    }

    public void OnNewBoost(InputAction.CallbackContext context) => ProfileUI.TookBoost();

    public void OnNewWeapon(InputAction.CallbackContext context) => ProfileUI.TookWeapon();

    #endregion
}
