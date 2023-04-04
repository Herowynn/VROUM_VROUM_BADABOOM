using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherProjectile : MonoBehaviour
{
    Rigidbody _targetRB;
    Rigidbody _projectileRB;
    public int FollowSpeed;
    Vector3 direction;
    public float _clock;
    bool _moved = false;
    private int _stageOne = 1, _stageTwo = 2;
    private int _stageOneSpeed = 500;
    private int _stageTwoSpeed = 50;
    private void Start()
    {
        _projectileRB = GetComponent<Rigidbody>();
    }
    public void Init(Rigidbody rb, Vector3 dir)
    {
        _targetRB = rb;
        direction = dir;

    }

    void Update()
    {
        if (_clock < _stageOne && _moved == false)
        {
            _projectileRB.AddForce(direction * _stageOneSpeed, ForceMode.Acceleration);
            _moved = true;
        }
        if (_targetRB != null && _clock > _stageOne && _clock < _stageTwo)
        {
            _projectileRB.velocity = new Vector3(0, 0, 0);
            transform.position = Vector3.Lerp(transform.position, _targetRB.gameObject.transform.position, FollowSpeed * Time.deltaTime);
            transform.LookAt(_targetRB.transform);
        }
        if(_clock > _stageTwo)
        {
            _projectileRB.AddForce(transform.forward * _stageTwoSpeed, ForceMode.Acceleration);
        }
        _clock += Time.deltaTime;
    }
}
