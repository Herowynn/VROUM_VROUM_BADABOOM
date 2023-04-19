using UnityEngine;

/// <summary>
/// This class contains the harvester and the players start positions, the nodes of the harvester path and the 
/// start position nodes of every round.
/// </summary>
public class Map : MonoBehaviour
{
    [Header("Instance")] 
    public string Name;
    public Transform HarvesterStartPosition;
    public Transform[] PlayerStartPositions;
    public Transform[] HarvesterNodes;
    public RoundNode[] RoundNodes;

    public void Load()
    {
        gameObject.SetActive(true);
    }
}
