using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody ProjectileRB;

    // Start is called before the first frame update

    private void Awake()
    {
        ProjectileRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(5*new Vector3(0, 1, 0));
    }
    public void Init(Vector3 direction)
    {
        ProjectileRB.AddForce(direction * _speed, ForceMode.Acceleration);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.GetComponent<CarController>() != null) Destroy(gameObject);
    }

}
