using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bump : Offensive
{
   

    public GameObject ProjectilePrefab;
    public LayerMask Ground;
    int _ground;
    [SerializeField] private List<Collider> _colliders = new List<Collider>();
   

    void Start()
    {
        _ground = (int)Mathf.Log(1f * Ground.value, 2f);
    }
    public override void Shoot()
    {
        foreach  (Collider collider in _colliders)
        {
            Vector3 dir = collider.GetComponent<Transform>().position - GetComponentInParent<CarController>().gameObject.transform.position;
            collider.gameObject.GetComponent<Rigidbody>().AddForce(dir*10, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_colliders.Contains(other) && other.gameObject.layer != _ground)
        {
            _colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);
    }
}