using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    /// <summary>
    /// This method plays the rotating saw sound effect and adds a force to the rigid body of this object.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="carLayerMask"></param>
    public void Init(Vector3 direction, LayerMask carLayerMask)
    {
        _source.clip = SawFlySound;
        _source.loop = true;
        _source.Play();

        _projectileRB.AddForce(direction * _speed, ForceMode.Acceleration);
        StartCoroutine(WaitBeforeAutoDestroy());
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(WaitBeforeActivateCollider());
    }

    /// <summary>
    /// This method verifies that a car is hit and plays sound effects and sets car variables accordingly
    /// if it is the case.
    /// </summary>
    /// <param name="other"></param>
/*    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<GlobalController>(out var carController))
        {
            carController.Source.clip = SawHitCarSounds[Random.Range(0, SawHitCarSounds.Length)];
            carController.Source.loop = false;
            carController.Source.Play();

            carController.HitBySaw = true;
            Destroy(gameObject);
        }
        else if (other.gameObject.GetComponent<DestructorComponent>())
            Destroy(gameObject);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<GlobalController>(out var carController))
        {
            carController.Source.clip = SawHitCarSounds[Random.Range(0, SawHitCarSounds.Length)];
            carController.Source.loop = false;
            carController.Source.Play();

            carController.HitBySaw = true;
            Destroy(gameObject);
        }
        else if (collision.gameObject.GetComponent<DestructorComponent>())
            Destroy(gameObject);
    }

    IEnumerator WaitBeforeAutoDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    IEnumerator WaitBeforeActivateCollider()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<BoxCollider>().enabled = true;

    }
}

