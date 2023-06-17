using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScoreManager is a manager that is used to add score to alive players.
/// When a player dies, this manager is triggered and alive players get score.
/// It is dynamic because you can change the amount of points gained per car crash.
/// This script will also trigger an event when the points are equal or above the necessary points to win a match.
/// </summary>

public class ScoreManager : MonoBehaviour
{
    [Header("GD")] 
    public int PointPerCrash;
    public int PointsToWin;
    
    //intern var
    public List<GlobalController> Players;

    public void InitiatePlayersForCurrentMatch()
    {
        Players = new List<GlobalController>();
        foreach (var playerGo in GameManager.Instance.PlayersManager.Players)
            Players.Add(playerGo.GetComponent<GlobalController>());
    }

    public bool IsFinished()
    {
        foreach (var player in Players)
        {
            if (player.Score >= PointsToWin)
                return true;
        }

        return false;
    }

    public void AddScoreToAlivePlayers()
    {
        foreach (var player in Players)
        {
            if (player.PlayerState == PlayerState.ALIVE)
                player.AddPointsToScore(PointPerCrash);
        }

        if (IsFinished())
            GameManager.Instance.TriggerEndGameAfterRoundEvent();
    }

    public void OrderPlayersAccordingToScore()
    {
        Players.Sort(delegate(GlobalController player1, GlobalController player2)
        {
            if (player1.Score == null && player2.Score == null) return 0;
            else if (player1.Score == null) return -1;
            else if (player2.Score == null) return 1;
            else return -player1.Score.CompareTo(player2.Score);
        });
    }
}
