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

    [Header("Audio")]
    public AudioClip MachineGunSound;
    AudioSource _source;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// This methods plays the machine gun shooting sound effect and start the shooting coroutine.
    /// </summary>
    public override void Shoot()
    {
        _source.clip = MachineGunSound;
        _source.loop = true;
        _source.Play();

        StartCoroutine(MinigunShoot(_fireRate));
    }

    void Update()
    {
        _direction = -transform.right;
    }

    /// <summary>
    /// This methods instantiate and shoot a machine gun projectile every "time" seconds during "_durationAfterActivation" seconds.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator MinigunShoot(float time)
    {
        _timeIncrementation = 0;
        while (_timeIncrementation < _durationAfterActivation)
        {
            GameObject go = Instantiate(ProjectilePrefab, _bulletSpawnPoint);
            go.transform.parent = null;
            go.GetComponent<Projectile>().Init(_direction, gameObject.GetComponentInParent<GlobalController>());
            _timeIncrementation += time;
            yield return new WaitForSeconds(time);
        }

        Destroy(gameObject);
    }
}