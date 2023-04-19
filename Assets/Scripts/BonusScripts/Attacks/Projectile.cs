using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ImpactForce;

    [SerializeField] float _speed;
    Rigidbody _projectileRB;
    [SerializeField] GlobalController _carControl;

    //public GameObject Particles;
    [Header("Audio")]
    public AudioClip[] MetalImpactSounds;
   
    private AudioSource _source;


    private void Awake()
    {
        _projectileRB = GetComponent<Rigidbody>();
        _source= GetComponent<AudioSource>();
    }
    public void Init(Vector3 direction, GlobalController cc)
    {
        _carControl = cc;
        _projectileRB.AddForce(direction * _speed, ForceMode.Acceleration);
        StartCoroutine(WaitBeforeAutoDestroy());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<GlobalController>(out var carControl))
        {
            Vector3 dir = collision.gameObject.transform.position - _carControl.transform.position;
            carControl.IsTouchedByMachineGun = true;
            carControl.ProjectileDirection = dir;
            //GameObject go = Instantiate(Particles, collision.gameObject.transform);
            //Destroy(go);

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
