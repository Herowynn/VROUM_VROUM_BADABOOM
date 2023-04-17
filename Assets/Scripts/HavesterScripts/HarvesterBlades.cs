using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterBlades : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        transform.RotateAround(transform.forward, -_rotationSpeed * Time.deltaTime);
    }
}
