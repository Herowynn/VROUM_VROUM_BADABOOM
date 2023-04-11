using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("GD")] 
    public int PointPerCrash;
    public int PointsToWin;
    
    //intern var
    private List<PlayerController> _players;

    public void InitiatePlayersForCurrentMatch()
    {
        _players = new List<PlayerController>();
        foreach (var playerGo in GameManager.Instance.Players)
        {
            _players.Add(playerGo.GetComponent<PlayerController>());
        }
    }
    
    public bool IsFinished()
    {
        foreach (var player in _players)
        {
            if (player.Score >= PointsToWin)
            {
                return true;
            }
        }
        
        return false;
    }

    public void AddScoreToAlivePlayers()
    {
        foreach (var player in _players)
        {
            if (player.PlayerState == PlayerState.ALIVE)
            {
                player.AddPointsToScore(PointPerCrash);
            }
        }
        
        if (IsFinished())
            GameManager.Instance.TriggerEndGameEvent();
    }
}
