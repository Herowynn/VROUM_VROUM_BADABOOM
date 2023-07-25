using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// RoundManager is a manager used to deal with the respawn of each player when there is only 1 left.
/// His role is to reset their position and status and the position and status of the harvester.
/// The respawn point is found by calculating which point is closer to the round winner, like in Blaze Rush,
/// and is using the script RoundNode to do so.
/// </summary>

public class RoundManager : MonoBehaviour
{
    #region Public Fields

    [Header("Info")] 
    public int RoundNumber;
    public int PlayersAlive;
    public bool GameFinished;

    [Header("GD")] 
    public float TimeToRestartRound;

    #endregion

    #region Private Fields

    private List<GlobalController> _playersToPlaceForNextRound;
    private List<RoundNode> _roundNodesForCurrentMap = new List<RoundNode>();
    private Harvester _harvesterForCurrentMap;

    #endregion

    private void Start()
    {
        RoundNumber = 0;
        _playersToPlaceForNextRound = new List<GlobalController>();
    }

    public IEnumerator StartRound()
    {
        yield return new WaitForSeconds(TimeToRestartRound);
        
        PlayersAlive = GameManager.Instance.PlayersManager.Players.Count;
        RoundNumber++;
        GameManager.Instance.GameState = GameState.RACING;
    }

    public void InitiateRoundNodesForCurrentMap()
    {
        _roundNodesForCurrentMap = GameManager.Instance.MapManager.CurrentMap.RoundNodes.ToList();
    }

    public void InitiateHarvesterForCurrentMap()
    {
        _harvesterForCurrentMap = GameManager.Instance.HarvesterManager.HarvesterRef;
    }
    
    public void PlayerDiedEvent(GlobalController player)
    {
        _playersToPlaceForNextRound.Add(player);

        player.DiedEvent();
        
        PlayersAlive--;
        
        GameManager.Instance.TriggerScoreAddEvent();
        
        if (IsRoundFinished() && GameManager.Instance.GameState != GameState.POST_GAME)
        {
            if (GameFinished)
            {
                GameManager.Instance.TriggerEndGameEvent();
                return;
            }
            
            GameManager.Instance.GameState = GameState.NOT_RACING;
            PrepareNextRound();
        }
    }
    
    private bool IsRoundFinished()
    {
        if (PlayersAlive <= 1)
            return true;

        return false;
    }

    private void PrepareNextRound()
    {
        GetRoundWinner();
        ClearEveryBonus();
        PlaceGameElementsForNextRound();
        GameManager.Instance.Camera.AddTargets();
        StartCoroutine(StartRound());
    }

    private void GetRoundWinner()
    {
        foreach (var player in GameManager.Instance.PlayersManager.Players)
        {
            if (player.GetComponent<GlobalController>().PlayerState == PlayerState.ALIVE)
            {
                _playersToPlaceForNextRound.Add(player.GetComponent<GlobalController>());
                if (player.GetComponent<AIController>())
                    player.GetComponent<AIController>().StopAllCoroutines();
                return;
            }
        }
    }

    private void PlaceGameElementsForNextRound()
    {
        RoundNode closestNode = 
            FindClosestNodeFromWinner(_playersToPlaceForNextRound[_playersToPlaceForNextRound.Count - 1].transform.position);
        
        PlaceHarvesterForNextRound(closestNode.HarvesterNode);
        PlacePlayersForNextRound(closestNode.Nodes);
    }
    
    private void PlaceHarvesterForNextRound(Transform harvesterTransform)
    {
        _harvesterForCurrentMap.ResetToTransform(harvesterTransform);
    }
    
    private void PlacePlayersForNextRound(Transform[] playersTransform)
    {
        int cpt = 0;
        for (int i = _playersToPlaceForNextRound.Count - 1; i >= 0; i--)
        {
            _playersToPlaceForNextRound[i].RebornEvent(playersTransform[cpt]);

            if (_playersToPlaceForNextRound[i].gameObject.TryGetComponent<AIController>(out var aiControl))
                aiControl.UpdateTargetNodeOnReborn(playersTransform[cpt]);

            cpt++;
        }
        
        _playersToPlaceForNextRound.Clear();
    }

    private void ClearEveryBonus()
    {
        GameManager.Instance.DestroyBonusEvent();
    }

    private RoundNode FindClosestNodeFromWinner(Vector3 winnerPosition)
    {
        RoundNode closestNode = null;

        float distance = float.MaxValue;
        
        for (int i = 0; i < _roundNodesForCurrentMap.Count; i++)
        {
            if (distance > Vector3.Distance(_roundNodesForCurrentMap[i].Nodes[0].transform.position, winnerPosition))
            {
                closestNode = _roundNodesForCurrentMap[i];
                distance = Vector3.Distance(_roundNodesForCurrentMap[i].Nodes[0].transform.position, winnerPosition);
            }
        }

        return closestNode;
    }

    public GameObject[] RealtimeCarsRanking(List<GameObject> alivePlayers)
    {
        int harvesterTargetNodeIndex = _harvesterForCurrentMap.TargetNode;
        List<Transform> nodesToFollow = _harvesterForCurrentMap.NodesToFollow;

        List<int> nodesIndexesFromHarvester = new List<int>();
        Dictionary<GameObject, NodeIndexAndDistance> nodeIndexAndDistanceDictionary = new Dictionary<GameObject, NodeIndexAndDistance>();
        GameObject[] result = new GameObject[alivePlayers.Count];

        for (int i = harvesterTargetNodeIndex; i < nodesToFollow.Count; i++)
            nodesIndexesFromHarvester.Add(i);

        for (int i = 0; i < harvesterTargetNodeIndex; i++)
            nodesIndexesFromHarvester.Add(i);

        foreach(GameObject player in alivePlayers)
        {
            float distance = float.MaxValue;
            int nodeIndex = -1;

            for (int i = 0; i < nodesToFollow.Count; i++)
            {
                Vector3 carPosition = new Vector3(player.transform.position.x, nodesToFollow[i].position.y, player.transform.position.z);
                float distanceToNode = Vector3.Distance(nodesToFollow[i].position, carPosition);

                if (distance > distanceToNode && Mathf.Abs(Vector3.Angle(nodesToFollow[i].transform.forward, carPosition - nodesToFollow[i].transform.position)) > 90f)
                {
                    nodeIndex = i;
                    distance = distanceToNode;
                }
            }

            NodeIndexAndDistance nodeIndexAndDistance = new NodeIndexAndDistance(nodeIndex, distance);
            nodeIndexAndDistanceDictionary[player] = nodeIndexAndDistance;
        }

        List<KeyValuePair<GameObject, NodeIndexAndDistance>> nodeIndexAndDistanceList = nodeIndexAndDistanceDictionary.ToList();

        nodeIndexAndDistanceList.Sort(delegate (KeyValuePair<GameObject, NodeIndexAndDistance> pair1, KeyValuePair<GameObject, NodeIndexAndDistance> pair2)
        {
            int car1AboveRoad = 0;
            int car2AboveRoad = 0;

            if (Physics.Raycast(pair1.Key.transform.position, Vector3.down, float.MaxValue, 1 << pair1.Key.GetComponent<GlobalController>().GroundLayerNumber))
                car1AboveRoad = 1;
            if (Physics.Raycast(pair2.Key.transform.position, Vector3.down, float.MaxValue, 1 << pair2.Key.GetComponent<GlobalController>().GroundLayerNumber))
                car2AboveRoad = 1;

            int result = car1AboveRoad - car2AboveRoad;

            // If both cars are above the road or if they are both above void, the ranking is determined by their location on the track compared to other's.
            // Otherwise, the car above the road is considered to be better placed than the car above the void (regardless of its position).
            if (result == 0)
            {
                result = nodesIndexesFromHarvester.IndexOf(pair1.Value.NodeIndex).CompareTo(nodesIndexesFromHarvester.IndexOf(pair2.Value.NodeIndex));
                if (result == 0)
                    return pair1.Value.DistanceFromNode.CompareTo(pair2.Value.DistanceFromNode);
                else
                    return -result;
        }
            else
            return -result;
    });

        for (int i = 0; i < nodeIndexAndDistanceList.Count; i++)
            result[i] = nodeIndexAndDistanceList[i].Key;

        return result;
    }
}

public struct NodeIndexAndDistance
{
    public int NodeIndex;
    public float DistanceFromNode;

    public NodeIndexAndDistance(int nodeIndex,float distanceFromNode)
    {
        NodeIndex = nodeIndex;
        DistanceFromNode = distanceFromNode;
    }
}