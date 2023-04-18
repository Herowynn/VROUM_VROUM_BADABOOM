using UnityEngine;

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
