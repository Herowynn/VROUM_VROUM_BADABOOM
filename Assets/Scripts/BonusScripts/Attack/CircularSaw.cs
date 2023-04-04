using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : Offensive
{
    [SerializeField] private Transform _bulletSpawnPoint;

    public GameObject ProjectilePrefab;


    public override void Shoot()
    {
        GameObject go = Instantiate(ProjectilePrefab, _bulletSpawnPoint);
        go.transform.parent = null;
        go.GetComponent<SawProjectile>().Init(GetComponentInParent<CarController>().gameObject.transform.forward);
        Destroy(gameObject);
    }
}
