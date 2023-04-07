using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissileLauncherProjectile : MonoBehaviour
{
    public float FollowSpeed;
    public float _clock;


    private Rigidbody _targetRB;
    private Rigidbody _projectileRB;
    private Vector3 direction;

    [SerializeField] float _launchingSpeed;
    [SerializeField] float _lastPhaseSpeed;
    float _stageOne = 1f;
    float _stageTwo = 2f;
    int _forceAdded = 500;
    float _dist;
    bool _isRotated = false;

    private void Start()
    {
        _projectileRB = GetComponent<Rigidbody>();
       
        //_dist = (_targetRB.transform.position - transform.position).magnitude;
    }

    public void Init(GameObject go)
    {
       if(go) _targetRB = go.GetComponent<Rigidbody>();
        direction = transform.forward;
    }

    void Update()
    {
        if (_clock < _stageOne)
        {
            
            _projectileRB.AddForce(direction * _forceAdded, ForceMode.Acceleration);
            _projectileRB.velocity = Vector3.ClampMagnitude(_projectileRB.velocity, _launchingSpeed);
        }

        if ( _clock > _stageOne && _clock < _stageTwo)
        {
            if (_targetRB != null)
            {

                _projectileRB.velocity = new Vector3(0, 0, 0);
                transform.position = Vector3.Lerp(transform.position, _targetRB.transform.position, FollowSpeed * Time.deltaTime) ;
                transform.LookAt(_targetRB.transform);
            }
            else
            {
                if (_isRotated == false)
                {
                    transform.Rotate(new Vector3(30, 0, 0));
                    direction = transform.forward;
                    _isRotated = true;
                }
                _projectileRB.AddForce(direction * -0.5f, ForceMode.Acceleration);
            }
            
        }

        if (_clock > _stageTwo)
        {
            if(_targetRB == null && _isRotated == true)
            {
                transform.Rotate(30, 0, 0);
                _isRotated = false;
            }
            _projectileRB.AddForce(transform.forward * _forceAdded*1.5f, ForceMode.Acceleration);
            _projectileRB.velocity = Vector3.ClampMagnitude(_projectileRB.velocity, _lastPhaseSpeed);
        }

        _clock += Time.deltaTime;
    }
}
