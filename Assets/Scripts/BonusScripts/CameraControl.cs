using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject target;
    public float FollowSpeed;
    public Vector3 offset;
    private Transform _targetTransform;

    void Start()
    {
        _targetTransform = target.transform;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _targetTransform.position + offset, FollowSpeed * Time.deltaTime);
        transform.LookAt(_targetTransform);
    }
}
