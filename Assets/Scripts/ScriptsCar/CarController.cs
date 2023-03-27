using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody SphereRB;
    public float forwardAccel;
    public float maximumSpeed;
    public float turnStrength;
    public float dragOnGround;
    public float dragInTheAir;
    public float wheelTurnSpeed;
    public float additionalEarthGravity;

    public LayerMask groundMask;
    public int groundLayerNumber;
    public float arrayRayLength;
    public Transform RayPoint;
    public GameObject RightBackWheel;
    public GameObject RightFrontWheel;
    public GameObject LeftBackWheel;
    public GameObject LeftFrontWheel;

    public float MaxWheelTurn = 25f;

    public float WheelRotation = 50f;

    public GameObject Arrow;
    public GameObject ArrowRotationCenter;

    public Transform AttackObject;
    public Transform BoostObject;
    public GameObject[] AttackList;
    public GameObject[] BoostList;


    private float _speedInput;
    private bool _isGrounded;
    private bool _canMove;
    private Vector3 _distArrowRayPoint;
    private Vector3 _wantedDirection;
    private float _yComponentWantedDirection;
    private Vector3 _carAltitudeOffset;

    private void Start()
    {
        SphereRB.transform.parent = null;

        _distArrowRayPoint = Arrow.transform.position - RayPoint.position;
        _yComponentWantedDirection = 0f;

        _carAltitudeOffset = new Vector3(0, transform.position.y - SphereRB.transform.position.y, 0);
    }

    private void Update()
    {
        _speedInput = 0f;
        _distArrowRayPoint = Arrow.transform.position - RayPoint.position;

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            _wantedDirection = new Vector3(Input.GetAxis("Horizontal"), _yComponentWantedDirection, Input.GetAxis("Vertical"));
            _speedInput = forwardAccel * 1000f;

            Vector3 wheelsRotationAxis = Quaternion.AngleAxis(90, transform.up) * transform.forward;

            LeftBackWheel.transform.RotateAround(wheelsRotationAxis, wheelTurnSpeed * Time.deltaTime);
            RightBackWheel.transform.RotateAround(wheelsRotationAxis, wheelTurnSpeed * Time.deltaTime);
            LeftFrontWheel.transform.RotateAround(wheelsRotationAxis, wheelTurnSpeed * Time.deltaTime);
            RightFrontWheel.transform.RotateAround(wheelsRotationAxis, wheelTurnSpeed * Time.deltaTime);

            if (_isGrounded)
            {
                Vector3 cross1 = Vector3.Cross(transform.forward, _wantedDirection);
                Vector3 cross2 = Vector3.Cross(_distArrowRayPoint, _wantedDirection);
                float carSignRotation = Mathf.Sign(cross1.y);
                float arrowSignRotation = Mathf.Sign(cross2.y);
                Vector3 velocity = Vector3.zero;

                if (Mathf.Abs(Mathf.Acos(Vector3.Dot(transform.forward.normalized, _wantedDirection.normalized))) > Mathf.Deg2Rad * 20f)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, carSignRotation * turnStrength * 100 * Time.deltaTime, 0f));
                    _canMove = false;
                }
                else
                {
                    transform.forward = _wantedDirection;
                    _canMove = true;
                }

                if (Mathf.Abs(Mathf.Acos(Vector3.Dot(ArrowRotationCenter.transform.forward.normalized, _wantedDirection.normalized))) > Mathf.Deg2Rad * 20f)
                    ArrowRotationCenter.transform.RotateAround(transform.up, carSignRotation * turnStrength * Time.deltaTime);
                else
                    ArrowRotationCenter.transform.forward = _wantedDirection;
            }
        }

        transform.position = SphereRB.transform.position + _carAltitudeOffset;

        // Keyboard attack boost button : left CTRL
        if (AttackObject.transform.childCount != 0 && Input.GetButton("Fire1"))
        {
            if (AttackObject.transform.GetComponentInChildren<Offensive>())
            {
                AttackObject.transform.GetChild(0).GetComponent<Offensive>().Shoot();
            }
        }
        // Keyboard speed boost button : left SHIFT
        if (BoostObject.transform.childCount != 0 && Input.GetButton("Fire3"))
        {
            if (BoostObject.transform.GetComponentInChildren<Booster>()) 
            {
                BoostObject.transform.GetChild(0).GetComponent<Booster>().Boost(SphereRB, this.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        _isGrounded = false;

        RaycastHit hit;

        if (Physics.Raycast(RayPoint.position, -transform.up, out hit, arrayRayLength, groundMask))
        {
            _isGrounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            if (hit.normal == new Vector3(0, 1, 0))
                _yComponentWantedDirection = 0;
            else
                _yComponentWantedDirection = transform.forward.y;
        }

        if (_isGrounded)
        {
            SphereRB.drag = dragOnGround;

            if (Mathf.Abs(_speedInput) > 0f && _canMove)
            {
                SphereRB.AddForce(_wantedDirection * _speedInput);
                SphereRB.velocity = Vector3.ClampMagnitude(SphereRB.velocity, maximumSpeed);
            }
        }
        else
        {
            SphereRB.drag = dragInTheAir;
            SphereRB.AddForce(new Vector3(0, -1, 0) * SphereRB.mass * additionalEarthGravity * 9.8f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bonus>() != null)
        {
            switch (collision.gameObject.GetComponent<Bonus>().Type)
            {
                case BonusType.Attack:
                    if (AttackObject.transform.childCount != 0) 
                        return;
                    else
                    {
                        Destroy(collision.gameObject);
                        Instantiate(AttackList[collision.gameObject.GetComponent<Bonus>().rndLvl], AttackObject);
                    }
                    break;
                case BonusType.Boost:
                    if (BoostObject.transform.childCount != 0) 
                        return;
                    else
                    {
                        Destroy(collision.gameObject);
                        Instantiate(BoostList[collision.gameObject.GetComponent<Bonus>().rndLvl], BoostObject);
                    }
                    break;
                default:
                    break;
            }
        }

        if (collision.gameObject.layer == groundLayerNumber && !_isGrounded)
            StartCoroutine(GetBackOnWheels());
    }

    public IEnumerator GetBackOnWheels()
    {
        yield return new WaitForSeconds(2f);
        transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
    }
}
