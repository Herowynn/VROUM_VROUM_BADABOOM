using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotgunProjectile : MonoBehaviour
{
    public float SlowSpeed;
    [SerializeField] private float _speed;
    private Rigidbody ProjectileRB;
    [SerializeField] CarController _carControl;

    private void Awake()
    {
        ProjectileRB = GetComponent<Rigidbody>();
    }
    public void Init(Vector3 direction)
    {
        ProjectileRB.AddForce(direction * _speed, ForceMode.Acceleration);
        GetComponent<SphereCollider>().enabled = false;
        Debug.Log(GetComponent<SphereCollider>().enabled);

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
        Debug.Log(GetComponent<SphereCollider>().enabled);
    }
}