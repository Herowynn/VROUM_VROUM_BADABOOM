using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameParameters : MonoBehaviour
{
    [Header("Instances")] 
    public TMP_Dropdown NbLocal;
    public TMP_Dropdown NbAi;
    public TMP_Dropdown AiDifficulty;
    public TMP_Dropdown MapSelection;
    public TMP_Dropdown ScoreToWin;

    private void Start()
    {
        NbLocal.ClearOptions();
        NbAi.ClearOptions();
        AiDifficulty.ClearOptions();
        MapSelection.ClearOptions();
        ScoreToWin.ClearOptions();
        MenuManager.Instance.SetDynamicDropdowns();
    }

    public void SetNbLocalEvent(int localIndex)
    {
        MenuManager.Instance.UpdateNbLocal(localIndex);
    }
    public void SetNbAiEvent(int aiIndex)
    {
        MenuManager.Instance.UpdateNbAi(aiIndex);
    }
    public void SetAiDifficultyEvent(int difficultyIndex)
    {
        MenuManager.Instance.UpdateAiDifficulty(difficultyIndex);
    }
    public void SetMapEvent(int mapIndex)
    {
        MenuManager.Instance.UpdateMap(mapIndex);
    }
    public void SetScoreToWinEvent(int scoreIndex)
    {
        MenuManager.Instance.UpdateScoreToWin(scoreIndex);
    }
}
