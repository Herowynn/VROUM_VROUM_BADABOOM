using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    [Header("GD")] 
    public int PointPerCrash;
    public int PointsToWin;
    
    //intern var
    public List<CarController> Players;

    public void InitiatePlayersForCurrentMatch()
    {
        Players = new List<CarController>();
        foreach (var playerGo in GameManager.Instance.PlayersManager.Players)
        {
            Players.Add(playerGo.GetComponent<CarController>());
        }
    }
    
    public bool IsFinished()
    {
        foreach (var player in Players)
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
        foreach (var player in Players)
        {
            if (player.PlayerState == PlayerState.ALIVE)
            {
                player.AddPointsToScore(PointPerCrash);
            }
        }
        
        if (IsFinished())
            GameManager.Instance.TriggerEndGameAfterRoundEvent();
    }

    public void OrderPlayersAccordingToScore()
    {
        Players.Sort(delegate(CarController player1, CarController player2)
        {
            if (player1.Score == null && player2.Score == null) return 0;
            else if (player1.Score == null) return -1;
            else if (player2.Score == null) return 1;
            else return -player1.Score.CompareTo(player2.Score);
        });
    }
}
