using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Offensive
{
    [SerializeField] Transform _bulletSpawnPoint;
    Vector3 _direction;

    public GameObject ProjectilePrefab;

    void Update()
    {
        _direction = -transform.right;
    }

    /// <summary>
    /// This method instantiates the shot gun projectile prefab and calls its initialization method.
    /// </summary>
    public override void Shoot()
    {
        GameObject go = Instantiate(ProjectilePrefab, _bulletSpawnPoint);
        go.transform.parent = null;
        go.GetComponent<ShotgunProjectile>().Init(_direction);
        Destroy(gameObject);
    }
}
