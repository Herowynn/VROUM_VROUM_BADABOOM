using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSaw : Offensive
{
    [SerializeField] private Transform _bulletSpawnPoint;

    public GameObject ProjectilePrefab;

    [Header("Audio")]
    public AudioClip HatchOpenSound;
    public AudioClip LaunchSawSound;
    AudioSource _source;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();

        _source.clip = HatchOpenSound;
        _source.Play();
    }

    public override void Shoot()
    {
        _source.clip = LaunchSawSound;
        _source.Play();

        GameObject go = Instantiate(ProjectilePrefab, _bulletSpawnPoint);
        go.transform.parent = null;
        go.GetComponent<SawProjectile>().Init(GetComponentInParent<GlobalController>().gameObject.transform.forward, GetComponentInParent<GlobalController>().CarLayerMask);
        StartCoroutine(WaitBeforeDestroy(go));
    }

    IEnumerator WaitBeforeDestroy(GameObject go)
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}
