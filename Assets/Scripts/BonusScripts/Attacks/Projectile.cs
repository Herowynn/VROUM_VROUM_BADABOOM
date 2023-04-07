using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ImpactForce;

    [SerializeField] float _speed;
    Rigidbody _projectileRB;
    [SerializeField] CarController _carControl;

    //public GameObject Particles;

    private void Awake()
    {
        _projectileRB = GetComponent<Rigidbody>();
    }
    public void Init(Vector3 direction, CarController cc)
    {
        _carControl = cc;
        _projectileRB.AddForce(direction * _speed, ForceMode.Acceleration);
        StartCoroutine(WaitBeforeAutoDestroy());
    }
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 dir = collision.gameObject.transform.position - _carControl.transform.position;

        if (collision.gameObject.TryGetComponent<CarController>(out var carControl))
        {
            carControl.IsTouchedByMachineGun = true;
            carControl.ProjectileDirection = dir;
            //GameObject go = Instantiate(Particles, collision.gameObject.transform);
            //Destroy(go);
            Destroy(gameObject);
        }
    }

    IEnumerator WaitBeforeAutoDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
