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
    public float PlayerPlacementOffset;
    
    //intern var
    private GameObject _roundWinner;
    private List<GameObject> _playersToPlaceOnNextRound;

    private void Start()
    {
        RoundNumber = 0;
    }

    public void StartRound()
    {
        PlayersAlive = GameManager.Instance.Players.Count;
        RoundNumber++;
        GameManager.Instance.GameState = GameState.RACING;
    }

    public void PlayerDiedEvent(GameObject playerGo)
    {
        _playersToPlaceOnNextRound.Add(playerGo);
        playerGo.SetActive(false);
        PlayersAlive--;
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
        PlacePlayers();
    }

    private void GetRoundWinner()
    {
        foreach (var player in GameManager.Instance.Players)
        {
            if (player.activeSelf)
                _roundWinner = player;
        }
    }

    private void PlacePlayers()
    { 
        for (int i = _playersToPlaceOnNextRound.Count - 1; i >= 0; i--)
        {
            _playersToPlaceOnNextRound[i].transform.position = _roundWinner.transform.position;
        }
    }
}
