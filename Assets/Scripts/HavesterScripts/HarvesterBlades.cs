using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterBlades : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private List<int> _ignoredLayersNumbers = new List<int>();

    private void Start()
    {
        _ignoredLayersNumbers.Add(HarvesterCoreGame.Instance.CarLayerNumber);
        _ignoredLayersNumbers.Add(HarvesterCoreGame.Instance.BonusLayerNumber);
    }

    private void Update()
    {
        transform.RotateAround(transform.forward, -_rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (_ignoredLayersNumbers.Contains(collision.gameObject.layer))
            {
                if (collision.gameObject.GetComponent<CarController>())
                    Destroy(collision.gameObject.GetComponent<CarController>().SphereRB.gameObject);

                Destroy(collision.gameObject);
            }
        }
    }
}
