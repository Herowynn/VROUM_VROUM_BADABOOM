using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public GameObject target;
    public float FollowSpeed, distance = 2;
    public Vector3 offset;
    private Transform _targetTransform;
    // Start is called before the first frame update
    void Start()
    {
        _targetTransform = target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _targetTransform.position+offset, FollowSpeed*Time.deltaTime);
        transform.LookAt(_targetTransform);
    }
}
