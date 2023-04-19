using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Info")] 
    public int RoundNumber;
    public int PlayersAlive;
    public bool GameFinished;

    [Header("GD")] 
    public float TimeToRestartRound;
    
    //intern var
    private List<GlobalController> _playersToPlaceForNextRound;
    private List<RoundNode> _roundNodesForCurrentMap = new List<RoundNode>();
    private Harvester _harvesterForCurrentMap;

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
        {
            return true;
        }

        return false;
    }

    private void PrepareNextRound()
    {
        GetRoundWinner();
        ClearEveryBonus();
        PlaceGameElementsForNextRound();
        StartCoroutine(StartRound());
    }

    private void GetRoundWinner()
    {
        foreach (var player in GameManager.Instance.PlayersManager.Players)
        {
            if (player.GetComponent<CarController>().PlayerState == PlayerState.ALIVE)
            {
                _playersToPlaceForNextRound.Add(player.GetComponent<CarController>());
                return;
            }
        }
    }

    private void PlaceGameElementsForNextRound()
    {
        RoundNode closestNode =
            FindClosestNodeFromWinner(_playersToPlaceForNextRound[^1].transform.position);
        
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
            //See if necessary
            if (_playersToPlaceForNextRound[i].gameObject.GetComponent<AIController>())
                _playersToPlaceForNextRound[i].gameObject.GetComponent<AIController>()
                    .SetTargetNode(_roundNodesForCurrentMap.IndexOf(playersTransform[0].parent.GetComponent<RoundNode>()));

            _playersToPlaceForNextRound[i].RebornEvent(playersTransform[cpt]);
            //_playersToPlaceForNextRound[i].gameObject.transform.position = closestNode.Nodes[cpt].transform.position;
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
}
