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
    private List<PlayerScore> _scores = new List<PlayerScore>();

    public bool IsFinished()
    {
        foreach (var playerScore in _scores)
        {
            if (playerScore.Score >= PointsToWin)
            {
                return true;
            }
        }
        
        return false;
    }

    public void AddPlayerScore(PlayerScore playerScore)
    {
        _scores.Add(playerScore);
    }
}
