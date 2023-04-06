using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Info")] 
    public int RoundNumber;
    public int PlayersAlive;

    [Header("GD")] 
    public float TimeToRestartRound;
    
    //intern var
    private GameObject _roundWinner;
    private List<GameObject> _playersToPlaceForNextRound;

    private void Start()
    {
        RoundNumber = 0;
        _playersToPlaceForNextRound = new List<GameObject>();
    }

    public IEnumerator StartRound()
    {
        yield return new WaitForSeconds(TimeToRestartRound);
        
        PlayersAlive = GameManager.Instance.Players.Count;
        RoundNumber++;
        GameManager.Instance.GameState = GameState.RACING;

        Debug.Log(RoundNumber);
    }

    public void PlayerDiedEvent(GameObject playerGo)
    {
        _playersToPlaceForNextRound.Add(playerGo);
        playerGo.SetActive(false);
        PlayersAlive--;

        if (IsRoundFinished())
        {
            PrepareNextRound();
        }
    }

    private bool IsRoundFinished()
    {
        if (PlayersAlive <= 1)
        {
            GameManager.Instance.GameState = GameState.NOT_RACING;
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
        foreach (var player in GameManager.Instance.Players)
        {
            if (player.activeSelf)
            {
                _playersToPlaceForNextRound.Add(player);
                return;
            }
        }
    }

    private void PlacePlayersForNextRound()
    {
        //TODO
        //Need to find closest node from winner
        RoundNode closestNode = GameManager.Instance.MapManager.CurrentMap.RoundNodes[0];
        
        for (int i = _playersToPlaceForNextRound.Count - 1, j = 0; i >= 0 && j < closestNode.Nodes.Length; i--, j++)
        {
            //See if necessary
            //_playersToPlaceForNextRound[i].transform.rotation = closestNode.Nodes[j].transform.rotation;
            _playersToPlaceForNextRound[i].transform.position = closestNode.Nodes[j].transform.position;
            _playersToPlaceForNextRound[i].SetActive(true);
        }
        
        _playersToPlaceForNextRound.Clear();
    }
}
