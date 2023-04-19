using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bump : Offensive
{
    private GameObject _bumpEffect;
    private CarController _carControl;
    private int _ground;
    private List<Collider> _colliders = new List<Collider>();

    [Header("Audio")]
    private AudioSource _source;
    public AudioClip SonicShotSound;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        _ground = (int)Mathf.Log(1f * GetComponentInParent<CarController>().GroundLayerMask.value, 2f);

        _bumpEffect = transform.GetChild(0).gameObject;
        _bumpEffect.SetActive(false);
    }

    /// <summary>
    /// This method pushes all the objects present in the effect zone.
    /// </summary>
    public override void Shoot()
    {
        _source.clip = SonicShotSound;
        _source.Play();

        foreach (Collider collider in _colliders)
        {
            if (collider)
            {
                Vector3 dir = collider.transform.position - GetComponentInParent<CarController>().gameObject.transform.position;

                if (collider.gameObject.TryGetComponent(out _carControl))
                {
                    _carControl.IsBumped = true;
                    _carControl.BumpDirection = dir;
                }
                else if(collider.gameObject.TryGetComponent<Bonus>(out var bonus))
                    bonus.GetComponent<Rigidbody>().AddForce(dir * 10, ForceMode.Impulse);
            }
        }

        StartCoroutine(WaitBeforeDestroy());
    }

    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    /// <summary>
    /// This method adds all the objects present in the effect zone to the objects list.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!_colliders.Contains(other) && other.gameObject.layer != _ground)
        {
            _colliders.Add(other);
        }
    }

    /// <summary>
    /// This method removes all the objects that get out of the zone effect from the objects list.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);
    }
}