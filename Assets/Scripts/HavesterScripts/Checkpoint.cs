using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool _harvesterAlreadyWalkedOn;

    private void Start()
    {
        _harvesterAlreadyWalkedOn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Harvester") && !_harvesterAlreadyWalkedOn)
        {
            HarvesterCoreGame.Instance.NextCheckpointIndex++;
            _harvesterAlreadyWalkedOn = true;
            StartCoroutine(ResetWalkedOnBool());
        }
    }
    
    private IEnumerator ResetWalkedOnBool()
    {
        yield return new WaitForSeconds(2f);
        _harvesterAlreadyWalkedOn = false;
    }
}
