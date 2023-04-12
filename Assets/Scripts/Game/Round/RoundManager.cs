using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<PlayerController> _playersToPlaceForNextRound;
    private RoundNode[] _roundNodesForCurrentMap;

    private void Start()
    {
        RoundNumber = 0;
        _playersToPlaceForNextRound = new List<PlayerController>();
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
        _roundNodesForCurrentMap = GameManager.Instance.MapManager.CurrentMap.RoundNodes;
    }
    
    public void PlayerDiedEvent(PlayerController player)
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
        PlacePlayersForNextRound();
        StartCoroutine(StartRound());
    }

    private void GetRoundWinner()
    {
        foreach (var player in GameManager.Instance.PlayersManager.Players)
        {
            if (player.GetComponent<PlayerController>().PlayerState == PlayerState.ALIVE)
            {
                _playersToPlaceForNextRound.Add(player.GetComponent<PlayerController>());
                return;
            }
        }
    }

    private void PlacePlayersForNextRound()
    {
        RoundNode closestNode =
            FindClosestNodeFromWinner(_playersToPlaceForNextRound[^1].transform.position);

        int cpt = 0;
        for (int i = _playersToPlaceForNextRound.Count - 1; i >= 0; i--)
        {
            //See if necessary
            //_playersToPlaceForNextRound[i].transform.rotation = closestNode.Nodes[j].transform.rotation;
            _playersToPlaceForNextRound[i].RebornEvent();
            _playersToPlaceForNextRound[i].gameObject.transform.position = closestNode.Nodes[cpt].transform.position;
            cpt++;
        }
        
        _playersToPlaceForNextRound.Clear();
    }

    private RoundNode FindClosestNodeFromWinner(Vector3 winnerPosition)
    {
        RoundNode closestNode = null;

        float distance = float.MaxValue;
        
        for (int i = 0; i < _roundNodesForCurrentMap.Length; i++)
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
