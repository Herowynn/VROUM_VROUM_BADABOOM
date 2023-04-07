using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileLauncher : Offensive
{
    public GameObject ProjectilePrefab;

    [SerializeField] Transform _bulletSpawnPoint;
    List<Collider> _colliders = new List<Collider>();

    public override void Shoot()
    {
        float closest = int.MaxValue;
        GameObject closestGO = null;
        for (int i = 0; i < _colliders.Count; i++)
        {
            
            if(closest > (_colliders[i].transform.position - GetComponentInParent<CarController>().gameObject.transform.position).magnitude)
            {
                closest = (_colliders[i].transform.position - GetComponentInParent<CarController>().gameObject.transform.position).magnitude;
                closestGO = _colliders[i].gameObject;
            }

        }
       
        GameObject go = Instantiate(ProjectilePrefab, _bulletSpawnPoint.position, GetComponentInParent<CarController>().gameObject.transform.rotation);
        go.transform.parent = null;
        go.GetComponent<MissileLauncherProjectile>().Init(closestGO);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_colliders.Contains(other) && other.gameObject.GetComponent<CarController>() != null) 
        {
            _colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);
    }
}

