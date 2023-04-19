using UnityEngine;

/// <summary>
/// RoundNode is a script used for every group of Node in the map.
/// Each map has groups of nodes, each group has respawn nodes (for the players to respawn)
/// and harvester Nodes (for the Harvester to respawn).
/// </summary>

public class RoundNode : MonoBehaviour
{
    [Header("Instance")] 
    public Transform HarvesterNode;
    public Transform[] Nodes;
}
