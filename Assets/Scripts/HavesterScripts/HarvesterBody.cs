using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterBody : MonoBehaviour
{
    private List<int> _ignoredLayersNumbers = new List<int>();

    private void Start()
    {
        _ignoredLayersNumbers.Add(HarvesterCoreGame.Instance.GroundLayerNumber);
        _ignoredLayersNumbers.Add(HarvesterCoreGame.Instance.HarvesterLayerNumber);
        _ignoredLayersNumbers.Add(HarvesterCoreGame.Instance.CheckPointLayerNumber);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null) 
        {
            if (!_ignoredLayersNumbers.Contains(collision.gameObject.layer))
                Destroy(collision.gameObject);
        }
    }
}
