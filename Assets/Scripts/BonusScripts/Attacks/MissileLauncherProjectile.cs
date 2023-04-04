using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherProjectile : MonoBehaviour
{
    public float FollowSpeed;
    public float _clock;


    private Rigidbody _targetRB;
    private Rigidbody _projectileRB;
    private Vector3 direction;

    [SerializeField] private float _launchingSpeed;
    [SerializeField] private float _lastPhaseSpeed;
    private float _stageOne = 1f;
    private float _stageTwo = 2f;
    private int _forceAdded = 1000;
    private float _dist;

    private void Start()
    {
        _projectileRB = GetComponent<Rigidbody>();
        _dist = (_targetRB.transform.position - transform.position).magnitude;
    }

    public void Init(Rigidbody rb, Vector3 dir)
    {
        _targetRB = rb;
        direction = dir;
    }

    void Update()
    {
        if (_clock < _stageOne)
        {
            _projectileRB.AddForce(direction * _forceAdded, ForceMode.Acceleration);
            _projectileRB.velocity = Vector3.ClampMagnitude(_projectileRB.velocity, _launchingSpeed);
        }

        if (_targetRB != null && _clock > _stageOne && _clock < _stageTwo)
        {
            _projectileRB.velocity = new Vector3(0, 0, 0);
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * _dist, FollowSpeed * Time.deltaTime);
            transform.LookAt(_targetRB.transform);
        }

        if (_clock > _stageTwo)
        {
            _projectileRB.AddForce(transform.forward * _forceAdded, ForceMode.Acceleration);
            _projectileRB.velocity = Vector3.ClampMagnitude(_projectileRB.velocity, _lastPhaseSpeed);
        }

        _clock += Time.deltaTime;
    }
}
