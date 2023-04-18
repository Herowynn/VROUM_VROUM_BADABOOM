using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Map : MonoBehaviour
{
    [Header("Instance")] 
    public string Name;
    public Transform HarvesterStartPosition;
    public Transform[] PlayerStartPositions;
    public Transform[] HarvesterNodes;
    public RoundNode[] RoundNodes;
    //public Harvester Harvester;

    public void Load()
    {
        gameObject.SetActive(true);
    }
}
