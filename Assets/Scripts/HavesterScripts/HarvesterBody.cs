using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterBody : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != HarvesterCoreGame.Instance.groundLayerNumber)
            Destroy(collision.gameObject);
    }
}
