using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Machine gun projectile class.
/// </summary>
public class Projectile : MonoBehaviour
{
    public float ImpactForce;

    [SerializeField] float _speed;
    Rigidbody _projectileRB;
    [SerializeField] CarController _carControl;

    [Header("Audio")]
    public AudioClip[] MetalImpactSounds;
   
    private AudioSource _source;

    private void Awake()
    {
        _projectileRB = GetComponent<Rigidbody>();
        _source= GetComponent<AudioSource>();
    }

    /// <summary>
    /// This method stores the CarController component of the shooting car and adds a force to this rigid body.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="cc"></param>
    public void Init(Vector3 direction, CarController cc)
    {
        _carControl = cc;
        _projectileRB.AddForce(direction * _speed, ForceMode.Acceleration);
        StartCoroutine(WaitBeforeAutoDestroy());
    }

    /// <summary>
    /// This method verifies that the object hit is a car and plays sound effects and sets car variables accordingly
    /// if it is the case.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<CarController>(out var carControl))
        {
            Vector3 dir = collision.gameObject.transform.position - _carControl.transform.position;
            carControl.IsTouchedByMachineGun = true;
            carControl.ProjectileDirection = dir;

            _source.clip = MetalImpactSounds[Random.Range(0,MetalImpactSounds.Length)];
            _source.Play();
            Destroy(gameObject);
        }
    }

    IEnumerator WaitBeforeAutoDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
