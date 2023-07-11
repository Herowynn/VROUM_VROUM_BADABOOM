using System.Collections.Generic;
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
    public List<Transform> PlayerStartPositions;
    public List<Transform> HarvesterNodes;
    public List<RoundNode> RoundNodes;

    public void Load()
    {
        HarvesterStartPosition = null;
        PlayerStartPositions.Clear();
        HarvesterNodes.Clear();
        RoundNodes.Clear();
        InitializeNodesArrays();

        gameObject.SetActive(true);
    }

    #region Nodes Arrays Initialization Methods

    private void InitializeNodesArrays()
    {
        List<GameObject> startPositions = GetStartPositions(gameObject);

        HarvesterStartPosition = startPositions[0].transform;

        for (int i = 1; i < startPositions.Count; i++)
            PlayerStartPositions.Add(startPositions[i].transform);

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject roadPart = gameObject.transform.GetChild(i).gameObject;

            if (roadPart.tag.Equals("RoadPart"))
            {
                RoundNode roundNode = GetRoundNode(roadPart);

                if (roundNode)
                {
                    RoundNodes.Add(roundNode);

                    List<GameObject> harvesterNodes = GetHarvesterNodes(roadPart);
                    foreach (GameObject harvesterNode in harvesterNodes)
                        HarvesterNodes.Add(harvesterNode.transform);
                }
            }
        }
    }

    private RoundNode GetRoundNode(GameObject roadPart)
    {
        for(int i = 0; i < roadPart.transform.childCount; i++)
        {
            GameObject child = roadPart.transform.GetChild(i).gameObject;

            if (child.GetComponent<RoundNode>())
                return child.GetComponent<RoundNode>();
        }

        return null;
    }

    private List<GameObject> GetHarvesterNodes(GameObject roadPart)
    {
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < roadPart.transform.childCount; i++)
        {
            GameObject child = roadPart.transform.GetChild(i).gameObject;

            if (child.tag.Equals("HarvesterNodesContainer"))
            {
                for(int j = 0; j < child.transform.childCount; j++)
                    result.Add(child.transform.GetChild(j).gameObject);
            }
        }

        return result;
    }

    private List<GameObject> GetStartPositions(GameObject road)
    {
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < road.transform.childCount; i++)
        {
            GameObject child = road.transform.GetChild(i).gameObject;

            if (child.tag.Equals("StartPositionsContainer"))
            {
                for (int j = 0; j < child.transform.childCount; j++)
                    result.Add(child.transform.GetChild(j).gameObject);
                break;
            }
        }

        return result;
    }

    #endregion
}
