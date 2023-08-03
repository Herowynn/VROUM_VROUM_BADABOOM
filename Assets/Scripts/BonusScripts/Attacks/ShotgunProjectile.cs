using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotgunProjectile : MonoBehaviour
{
    public LayerMask GroundLayerMask;

    [SerializeField] private float _force;

    [SerializeField] GlobalController _carControl;
    private Rigidbody ProjectileRB;
    private int _ground;

    [Header("Audio")]
    public AudioClip ShootSound;
    public AudioClip[] BounceSounds;
    private AudioSource _source;

    private void Awake()
    {
        ProjectileRB = GetComponent<Rigidbody>();
        _source = gameObject.AddComponent<AudioSource>();
        _ground = (int)Mathf.Log(1f * GroundLayerMask.value, 2f);
    }

    /// <summary>
    /// This method plays the shot gun projectile sound effect and adds force to the rigid body of this object.
    /// It also calls the WaitBeforeActivateCollider() coroutine.
    /// </summary>
    /// <param name="direction"></param>
    public void Init(Vector3 direction)
    {
        ProjectileRB.AddForce(direction * _force);
        GetComponent<SphereCollider>().enabled = false;
        _source.clip = ShootSound;
        _source.loop = false;
        _source.Play();

        StartCoroutine(WaitBeforeActivateCollider());
    }

    /// <summary>
    /// this method verifies the type of object hit :
    /// If it is a car, sets changes the slow factor applied to the car speed and calls the WaitBeforeNormalSpeed() coroutine.
    /// If it is an object with the ground layer, it plays a bounce sound effect.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out _carControl))
        {
            _carControl.SlowFactor = 0.5f;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(WaitBeforeNormalSpeedAndDestroy());
        }
        else if (collision.gameObject.layer != _ground && !collision.gameObject.GetComponent<DestructorComponent>())
        {
            _source.clip = BounceSounds[Random.Range(0, BounceSounds.Length)];
            _source.loop = false;
            _source.Play();
        }
        else if (collision.gameObject.GetComponent<DestructorComponent>())
        {
            _source.Stop();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This coroutine sets the slow factor of the car hit to its original value after a certain amount of time.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBeforeNormalSpeedAndDestroy()
    {
        yield return new WaitForSeconds(5f);
        _carControl.SlowFactor = 1f;
        Destroy(gameObject);
    }

    /// <summary>
    /// This coroutine enable the sphere collider of this projectile after a certain amount of time to avoid
    /// to hit the car that shot this shotgun projectile.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBeforeActivateCollider()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SphereCollider>().enabled = true;
    }
}