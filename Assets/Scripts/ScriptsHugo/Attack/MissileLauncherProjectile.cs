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
        if (_clock < 1 && _moved == false)
        {
            _projectileRB.AddForce(direction * 500, ForceMode.Acceleration);
            _moved = true;
        }
        if (_targetRB != null && _clock > 1)
        {
            _projectileRB.velocity = new Vector3(0, 0, 0);
            transform.position = Vector3.Lerp(transform.position, _targetRB.gameObject.transform.position, FollowSpeed * Time.deltaTime);
            transform.LookAt(_targetRB.transform);
        }
        _clock += Time.deltaTime;
    }
}
