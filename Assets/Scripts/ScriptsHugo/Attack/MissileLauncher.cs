using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileLauncher : Offensive
{
    public GameObject ProjectilePrefab;
    private List<Collider> _colliders = new List<Collider>();
    [SerializeField] private Transform _bulletSpawnPoint;

    public override void Shoot()
    {
        float closest = 0;
        GameObject ClosestGO= null;
        for (int i = 0; i < _colliders.Count; i++)
        {
            
            if(closest > (_colliders[i].transform.position - GetComponentInParent<CarController>().gameObject.transform.position).magnitude)
            {
                ClosestGO = _colliders[i].gameObject;
            }

        }
        GameObject go = Instantiate(ProjectilePrefab, _bulletSpawnPoint);
        go.transform.parent = null;
        //go.GetComponent<MissileLauncherProjectile>().Init(ClosestGO.GetComponent<Rigidbody>());
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_colliders.Contains(other) && other.gameObject.GetComponent<CarController>() !=null)
        {
            _colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);
    }
}

