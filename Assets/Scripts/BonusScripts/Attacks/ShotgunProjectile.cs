using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotgunProjectile : MonoBehaviour
{
    public LayerMask GroundLayerMask;

    [SerializeField] float _speed;
    Rigidbody ProjectileRB;
    [SerializeField] CarController _carControl;
    int _ground;

    [Header("Audio")]
    public AudioClip ShootSound;
    public AudioClip[] BounceSounds;
    AudioSource _source;

    private void Awake()
    {
        ProjectileRB = GetComponent<Rigidbody>();
        _source = gameObject.AddComponent<AudioSource>();
        _ground = (int)Mathf.Log(1f * GroundLayerMask.value, 2f);
    }
    public void Init(Vector3 direction)
    {
        ProjectileRB.AddForce(direction * _speed, ForceMode.Acceleration);
        GetComponent<SphereCollider>().enabled = false;
        Debug.Log(GetComponent<SphereCollider>().enabled);
        _source.clip = ShootSound;
        _source.loop = false;
        _source.Play();

        StartCoroutine(WaitBeforeActivateCollider());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out _carControl))
        {
            _carControl.SlowFactor = .5f;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(WaitBeforeNormalSpeed());
        }
        else if(collision.gameObject.layer != _ground)
        {
            _source.clip = BounceSounds[Random.Range(0, BounceSounds.Length)];
            _source.loop = false;
            _source.Play();
        }
    }

    IEnumerator WaitBeforeNormalSpeed()
    {
        yield return new WaitForSeconds(5f);
        _carControl.SlowFactor = 1f;
        Destroy(gameObject);
    }

    IEnumerator WaitBeforeActivateCollider()
    {
        yield return new WaitForSeconds(.2f);
        GetComponent<SphereCollider>().enabled = true;
        
    }
}