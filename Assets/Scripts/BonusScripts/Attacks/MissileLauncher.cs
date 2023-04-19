using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileLauncher : Offensive
{
    public GameObject ProjectilePrefab;

    [SerializeField] Transform _bulletSpawnPoint;
    List<Collider> _colliders = new List<Collider>();

    [Header("Audio")]
    public AudioClip HatchOpenSound;
    AudioSource _source;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
        _source.clip = HatchOpenSound;
        _source.Play();
    }

    /// <summary>
    /// This methods gets the closest object present in the target zone of the launcher, instantiate the missile and calls
    /// its initialization method before destroying this launcher object.
    /// </summary>
    public override void Shoot()
    {
        float closest = int.MaxValue;
        GameObject closestGO = null;
        for (int i = 0; i < _colliders.Count; i++)
        {
            if(closest > (_colliders[i].transform.position - GetComponentInParent<GlobalController>().gameObject.transform.position).magnitude)
            {
                closest = (_colliders[i].transform.position - GetComponentInParent<GlobalController>().gameObject.transform.position).magnitude;
                closestGO = _colliders[i].gameObject;
            }
        }
       
        GameObject go = Instantiate(ProjectilePrefab, _bulletSpawnPoint.position, GetComponentInParent<GlobalController>().gameObject.transform.rotation);
        go.transform.parent = null;
        go.GetComponent<MissileLauncherProjectile>().Init(closestGO, GetComponentInParent<GlobalController>().gameObject.transform.position);

        Destroy(gameObject);
    }

    /// <summary>
    /// This method adds all the objects present in the target zone of the launcher to the objects list.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!_colliders.Contains(other) && other.gameObject.GetComponent<GlobalController>() != null) 
        {
            _colliders.Add(other);
        }
    }

    /// <summary>
    /// This method removes all the objects that get out of the target zone from the objects list.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);
    }
}

