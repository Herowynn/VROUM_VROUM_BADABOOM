using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bump : Offensive
{
    GameObject _bumpEffect;
    CarController _carControl;
    int _ground;
    List<Collider> _colliders = new List<Collider>();

    [Header("Audio")]
    AudioSource _source;
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

    public override void Shoot()
    {
        _bumpEffect.SetActive(true);

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

    private void OnTriggerEnter(Collider other)
    {
        if (!_colliders.Contains(other) && other.gameObject.layer != _ground)
        {
            _colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);
    }
}