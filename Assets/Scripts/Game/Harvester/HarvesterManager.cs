using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterManager : MonoBehaviour
{
    [Header("Instances")] 
    public GameObject HarvesterPrefab;
    public GameObject HarvesterGoRef;
    public Harvester HarvesterRef;


    public void InstantiateHarvester()
    {
        HarvesterGoRef = Instantiate(HarvesterPrefab, GameManager.Instance.MapManager.CurrentMap.HarvesterStartPosition.position,
            Quaternion.identity, transform);

        HarvesterRef = HarvesterGoRef.GetComponent<Harvester>();
        HarvesterRef.InitiateNodesToFollow(GameManager.Instance.MapManager.CurrentMap.HarvesterNodes);
    }
}
