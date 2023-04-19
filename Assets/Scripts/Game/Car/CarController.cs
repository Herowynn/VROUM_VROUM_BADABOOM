using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    #region Public Fields

    [Header("GD")]
    public Rigidbody SphereRB;
    public float ForwardAccel;
    public float MaximumSpeed;
    public float TurnStrength;
    public float DragOnGround;
    public float DragInTheAir;
    public float WheelTurnSpeed;
    public float AdditionalEarthGravity;
    public float SlowFactor = 1f;

    [Header("Layers")]
    public LayerMask GroundLayerMask;
    public LayerMask CarLayerMask;
    public int GroundLayerNumber;
    public float ArrayRayLength;
    public Transform RayPoint;
    public GameObject RightBackWheel;
    public GameObject RightFrontWheel;
    public GameObject LeftBackWheel;
    public GameObject LeftFrontWheel;
    
    [Header("Bonus Effects")]
    public bool HitBySaw = false;
    public bool IsBumped;
    public bool IsExploded;
    public bool IsTouchedByMachineGun;
    public int BumpForce = 10000;
    public int ProjectileForce = 100;
    public Vector3 BumpDirection;
    public Vector3 ExplosionDirection;
    public Vector3 ProjectileDirection;

    [Header("Arrow")]
    public GameObject Arrow;
    public GameObject ArrowRotationCenter;

    [Header("Bonus")]
    public Transform AttacksContainer;
    public Transform BoostsContainer;
    public GameObject[] AttackList;
    public GameObject[] BoostList;
    
    [Header("Audio")]
    public AudioSource Source;
    public AudioClip GroundHitSound;
    public AudioClip BaseSound;
    public AudioClip MovingSound;
    
    [Header("Info")]
    public Color Color;
    public PlayerState PlayerState;
    
    [Header("Instance")]
    public GameObject Visual;
    public GameObject SphereReference;
    public MeshRenderer BodyColor;
    public List<Material> CarColors;

    [Header("Score")]
    //Need to move to separate file (?)
    public int Score;

    //Need to move to separate file (?)
    [Header("UI")]
    public PointsUI PointsUI;
    public ProfileUI ProfileUI;

    #endregion

    #region Private Fields

    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    private Vector2 _movementInput;
    bool _isGrounded;
    bool _wasGrounded;
    bool _canMove;
    Vector3 _distArrowRayPoint;
    Vector3 _wantedDirection;
    float _yComponentWantedDirection;
    Vector3 _carAltitudeOffset;
    private Vector3 _lastRebornPosition;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SphereRB.transform.parent = null;

        _distArrowRayPoint = Arrow.transform.position - RayPoint.position;
        _yComponentWantedDirection = 0f;

        _carAltitudeOffset = new Vector3(0, transform.position.y - SphereRB.transform.position.y, 0);



        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();

        CreatePlayerUis();

        PointsUI.ChangeVisualColoration(Color);
        PointsUI.ChangePointsCount(Score);
        BodyColor.material = CarColors[GameManager.Instance.PlayersManager.PlayerColors.IndexOf(Color)];

        for (int i = 0; i < GameManager.Instance.MapManager.CurrentMap.PlayerStartPositions.Length; i++)
        {
            if (transform.GetSiblingIndex() == i)
                _lastRebornPosition = GameManager.Instance.MapManager.CurrentMap.PlayerStartPositions[i].position;
        }
    }

    private void Update()
    {
        Debug.Log(SlowFactor);
        _distArrowRayPoint = Arrow.transform.position - RayPoint.position;

        if (GameManager.Instance.GameState == GameState.RACING && PlayerState == PlayerState.ALIVE && _movementInput != Vector2.zero)
        {
            Arrow.SetActive(true);

            _wantedDirection = new Vector3(_movementInput.x, _yComponentWantedDirection, _movementInput.y);

            Vector3 wheelsRotationAxis = Quaternion.AngleAxis(90, transform.up) * transform.forward;

            LeftBackWheel.transform.Rotate(wheelsRotationAxis, WheelTurnSpeed * Time.deltaTime);
            RightBackWheel.transform.Rotate(wheelsRotationAxis, WheelTurnSpeed * Time.deltaTime);
            LeftFrontWheel.transform.Rotate(wheelsRotationAxis, WheelTurnSpeed * Time.deltaTime);
            RightFrontWheel.transform.Rotate(wheelsRotationAxis, WheelTurnSpeed * Time.deltaTime);

            if (_isGrounded)
            {
                Vector3 cross1 = Vector3.Cross(transform.forward, _wantedDirection);
                Vector3 cross2 = Vector3.Cross(_distArrowRayPoint, _wantedDirection);
                float carSignRotation = Mathf.Sign(cross1.y);

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
        
        _isGrounded = false;
        RaycastHit hit;

        if (Physics.Raycast(RayPoint.position, -transform.up, out hit, ArrayRayLength, GroundLayerMask))
        {
            _isGrounded = true;

            
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            if (hit.normal == new Vector3(0, 1, 0))
                _yComponentWantedDirection = 0;
            else
                _yComponentWantedDirection = transform.forward.y;
        }

        if (HitBySaw && _isGrounded)
        {
            SphereRB.AddForce(transform.up * 20000, ForceMode.Impulse);
            StartCoroutine(WaitBeforeSawEnd());
        }

        if (HitBySaw && !_isGrounded)
        {
            transform.Rotate(30 * new Vector3(0, 1, 0));
        }

        if (IsExploded && _isGrounded)

        {
            SphereRB.AddForce((ExplosionDirection + new Vector3(0, 100, 0)) * 200, ForceMode.Impulse);
            StartCoroutine(WaitBeforeMissileEnd());
        }

        if (IsExploded && !_isGrounded)
        {
            transform.Rotate(30 * new Vector3(1, 0, 0));
        }

        if (_isGrounded)
        {
            SphereRB.drag = DragOnGround;

            if (_canMove && _movementInput != Vector2.zero)

            {
                SphereRB.AddForce(_wantedDirection * ForwardAccel);
                SphereRB.velocity = Vector3.ClampMagnitude(SphereRB.velocity, MaximumSpeed * SlowFactor);
                Source.clip = MovingSound;
                
            }
        }
        else
        {
            SphereRB.drag = DragInTheAir;
            SphereRB.AddForce(new Vector3(0, -1, 0) * SphereRB.mass * AdditionalEarthGravity * 9.8f);
            Source.clip = BaseSound;
        }

        if (IsBumped)
        {
            SphereRB.AddForce(BumpDirection * BumpForce, ForceMode.Impulse);
            IsBumped = false;
        }

        if (IsTouchedByMachineGun)
        {
            SphereRB.AddForce(ProjectileDirection * ProjectileForce, ForceMode.Impulse);
            IsTouchedByMachineGun = false;
        }

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bonus>() != null)
        {
            switch (collision.gameObject.GetComponent<Bonus>().Type)
            {
                case BonusType.Attack:
                    if (AttacksContainer.transform.childCount != 0)

                        return;
                    Instantiate(AttackList[collision.gameObject.GetComponent<Bonus>().RndLvl], AttacksContainer);
                    ProfileUI.TookWeapon();
                    break;
                case BonusType.Boost:
                    if (BoostsContainer.transform.childCount != 0)

                        return;
                    Instantiate(BoostList[collision.gameObject.GetComponent<Bonus>().RndLvl], BoostsContainer);
                    ProfileUI.TookBoost();
                    break;
            }



            Destroy(collision.gameObject);
        }

        if (collision.gameObject.layer == GroundLayerNumber && !_isGrounded)
            StartCoroutine(GetBackOnWheels());
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DestructorComponent>())
        {
            GameManager.Instance.TriggerPlayerDestructionEvent(this);
        }
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

    #region Related to state in-game

    public void DiedEvent()
    {
        SphereReference.SetActive(false);
        Visual.SetActive(false);
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _boxCollider.enabled = false;
        PlayerState = PlayerState.DEAD;
    }

    public void RebornEvent(Transform positionOnReborn)
    {
        ClearMyBonus();

        ProfileUI.ResetProfile();

        SphereReference.SetActive(true);
        Visual.SetActive(true);
        _rigidbody.constraints = RigidbodyConstraints.None;
        _boxCollider.enabled = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        PlayerState = PlayerState.ALIVE;

        transform.rotation = positionOnReborn.rotation;
        SphereRB.velocity = Vector3.zero;
        SphereReference.transform.position = positionOnReborn.position;
        _lastRebornPosition = positionOnReborn.position;
    }

    public void AddPointsToScore(int points)
    {
        Score += points;
        PointsUI.ChangePointsCount(Score);
    }
    
    private IEnumerator WaitBeforeSawEnd()
    {
        yield return new WaitForSeconds(1.2f);
        HitBySaw = false;
    }

    private IEnumerator WaitBeforeMissileEnd()
    {
        yield return new WaitForSeconds(1.2f);
        IsExploded = false;
    }

    private IEnumerator GetBackOnWheels()
    {
        yield return new WaitForSeconds(2f);
        transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
    }

    private void ClearMyBonus()
    {
        if (BoostsContainer.transform.childCount != 0)
            Destroy(BoostsContainer.transform.GetChild(0).gameObject);

        if (AttacksContainer.transform.childCount != 0)
            Destroy(AttacksContainer.transform.GetChild(0).gameObject);
    }

    #endregion

    #region Related to UI

    private void CreatePlayerUis()
    {
        GameManager.Instance.TriggerUiCreationForPlayerEvent(this);
    }

    #endregion
}
