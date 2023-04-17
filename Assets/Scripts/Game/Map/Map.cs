using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Instance")] 
    public string Name;
    public Transform[] StartPositions;
    public RoundNode[] RoundNodes;
    public Harvester Harvester;

    public void Load()
    {
        gameObject.SetActive(true);
    }
}
