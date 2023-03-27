using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineGun : Offensive
{
    [SerializeField] private float _durationAfterActivation;
    [SerializeField] private float _fireRate;
    [SerializeField] private Transform _bulletSpawnPoint;
    private Vector3 _direction;
    private float _timeIncrementation;

    public GameObject ProjectilePrefab;

    public override void Shoot()
    {
        StartCoroutine(MinigunShoot(_fireRate));
    }

    void Update()
    {
        _direction = -transform.right;
    }
    IEnumerator MinigunShoot(float time)
    {
        _timeIncrementation = 0;
        while (_timeIncrementation < _durationAfterActivation)
        {
            GameObject go = Instantiate(ProjectilePrefab, _bulletSpawnPoint);
            go.transform.parent = null;
            go.GetComponent<Projectile>().Init(_direction);
            _timeIncrementation += time;
            yield return new WaitForSeconds(time);
        }
        Debug.Log(_timeIncrementation);
        Destroy(gameObject);
    }
}