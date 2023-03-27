using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : Offensive
{
    [SerializeField] private float _durationAfterActivation;
    [SerializeField] private float _fireRate;
    [SerializeField] private Transform _bulletSpawnPoint;
    private Vector3 _direction;

    public GameObject ProjectilePrefab;

    void Update()
    {
        _direction = -transform.right;
    }
    public override void Shoot()
    {
        GameObject go = Instantiate(ProjectilePrefab, _bulletSpawnPoint);
        go.transform.parent = null;
        go.GetComponent<Projectile>().Init(_direction);
        Destroy(gameObject);
    }
}
