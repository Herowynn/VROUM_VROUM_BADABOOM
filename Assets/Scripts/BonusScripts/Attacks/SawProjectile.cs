using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawProjectile : MonoBehaviour
{
    [SerializeField] float _speed;
    Rigidbody _projectileRB;
    int _carLayer;
    private void Awake()
    {
        _projectileRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(30 * new Vector3(0, 1, 0));
    }

    public void Init(Vector3 direction, LayerMask carLayerMask)
    {
        _projectileRB.AddForce(direction * _speed, ForceMode.Acceleration);
        _carLayer = (int)Mathf.Log(1f * carLayerMask.value, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<CarController>(out var carController))
        {
            carController.HitBySaw = true;
        }

        //if (other.gameObject.layer == _carLayer)
        //{
        //    other.GetComponent<CarController>(). // AddForce(transform.up * 200, ForceMode.Impulse);
        //    StartCoroutine(WaitTime(.05f, other));
        //}
    }

    IEnumerator WaitTime(float time, Collider other)
    {
        yield return new WaitForSeconds(time);
        other.gameObject.GetComponent<CarController>().HitBySaw = true;
        Destroy(gameObject);
    }

}

