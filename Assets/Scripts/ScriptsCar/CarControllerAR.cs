using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    public float GroundRayLength = 0.5f;
    public Transform RayPoint;

    public Transform LeftFrontWheel, RightFrontWheel, LeftBackWheel, RightBackWheel;
    public float MaxWheelTurn = 25f;

    public float WheelRotation = 50f;

    public GameObject Arrow;
    public GameObject ArrowRotationCenter;


    private float _speedInput, _turnInput;
    private float _arrayRayLength;
    private bool _isGrounded;
    private bool _canMove;
    private Vector3 _distArrowRayPoint;
    private Vector3 _wantedDirection;
    private float _yComponentWantedDirection;

    private void Start()
    {
        SphereRB.transform.parent = null;

        _distArrowRayPoint = Arrow.transform.position - RayPoint.position;
        _arrayRayLength = _distArrowRayPoint.magnitude + 1f;
        _yComponentWantedDirection = 0f;
    }

    private void Update()
    {
        _speedInput = 0f;
        _distArrowRayPoint = Arrow.transform.position - RayPoint.position;

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            _wantedDirection = new Vector3(Input.GetAxis("Horizontal"), _yComponentWantedDirection, Input.GetAxis("Vertical"));
            _speedInput = forwardAccel * 1000f;

            LeftBackWheel.transform.Rotate(0f, WheelRotation, 0f);
            RightBackWheel.transform.Rotate(0f, WheelRotation, 0f);
            LeftFrontWheel.transform.Rotate(0f, WheelRotation, 0f);
            RightFrontWheel.transform.Rotate(0f, WheelRotation, 0f);

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

        transform.position = SphereRB.transform.position;
    }

    private void FixedUpdate()
    {
        _isGrounded = false;

        RaycastHit hit;

        if (Physics.Raycast(RayPoint.position, -transform.up, out hit, GroundRayLength, groundMask))
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

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == groundLayerNumber && !_isGrounded)
            StartCoroutine(GetBackOnWheels());
    }

    public IEnumerator GetBackOnWheels()
    {
        yield return new WaitForSeconds(2f);
        transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
    }*/
}
