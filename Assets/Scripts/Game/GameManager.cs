using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        LoadPreGame();
    }

    public GameState GameState;
    public MultipleInputManager MultipleInputManager;
    public UIManager UIManager;

    #region Game State

    public void LoadPreGame()
    {
        GameState = GameState.NOT_RACING;
        UIManager.DisplayInputMenu();
    }
    
    public void StartGame()
    {
        GameState = GameState.RACING;
        UIManager.DisplayGameUI();
    }

    #endregion
}
