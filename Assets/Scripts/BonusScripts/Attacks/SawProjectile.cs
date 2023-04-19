using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawProjectile : MonoBehaviour
{
    [SerializeField] float _speed;
    Rigidbody _projectileRB;

    [Header("Audio")]
    public AudioClip SawFlySound;
    public AudioClip[] SawHitCarSounds;
    AudioSource _source;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
        _projectileRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(30 * new Vector3(0, 1, 0));
    }

    public void Init(Vector3 direction, LayerMask carLayerMask)
    {
        _source.clip = SawFlySound;
        _source.loop = true;
        _source.Play();

        _projectileRB.AddForce(direction * _speed, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<GlobalController>(out var carController))
        {
            carController.Source.clip = SawHitCarSounds[Random.Range(0, SawHitCarSounds.Length)];
            carController.Source.loop = false;
            carController.Source.Play();

            carController.HitBySaw = true;
            Destroy(gameObject);
        }
    }
}

