using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterBody : MonoBehaviour
{
    private List<int> _ignoredLayersNumbers = new List<int>();

    private void Start()
    {
        _ignoredLayersNumbers.Add(HarvesterCoreGame.Instance.CarLayerNumber);
        _ignoredLayersNumbers.Add(HarvesterCoreGame.Instance.BonusLayerNumber);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null) 
        {
            if (_ignoredLayersNumbers.Contains(collision.gameObject.layer))
                Destroy(collision.gameObject);
        }
    }
}
