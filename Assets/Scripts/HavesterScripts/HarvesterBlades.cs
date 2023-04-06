using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterBlades : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Harvester enters in collision with an object !");
        if (collision.gameObject.layer != HarvesterCoreGame.Instance.groundLayerNumber)
            Destroy(collision.gameObject);
    }
}
