using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherProjectile : MonoBehaviour
{
    Rigidbody _targetRB;
    Rigidbody _projectileRB;
    public int FollowSpeed;
    Vector3 direction;

    private void Start()
    {
          _projectileRB= GetComponent<Rigidbody>();
    }
    public void Init(Rigidbody rb)
    {
        _targetRB= rb;
      
    }
    public void InitNoTarget(Vector3 dir)
    {
        direction = dir;
        
    }
    void Update()
    {
        if (_targetRB == null) _projectileRB.AddForce(direction * FollowSpeed, ForceMode.Acceleration); ;
        if (_targetRB != null)
        {
            transform.position = Vector3.Lerp(transform.position, _targetRB.gameObject.transform.position, FollowSpeed * Time.deltaTime);
            transform.LookAt(_targetRB.transform);
        }
    }
}
