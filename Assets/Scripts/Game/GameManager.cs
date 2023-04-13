using System;
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
        
        
    }

    [Header("Instance")]
    public MultipleInputManager MultipleInputManager;
    public UIManager UIManager;
    public CameraController Camera;
    public ScoreManager ScoreManager;
    public RoundManager RoundManager;
    public MapManager MapManager;
    public PlayersManager PlayersManager;

    [Header("Info")]
    public GameState GameState;

    private void Start()
    {
        //LoadPreGame();
        //test
        StartGame();
    }

    #region Game State

    public void LoadPreGame()
    {
        GameState = GameState.PRE_GAME;
        UIManager.DisplayInputMenu();
    }

    public void LoadPauseGame()
    {
        GameState = GameState.NOT_RACING;
        UIManager.DisplayInputMenu();
    }
    
    public void StartGame()
    {
        UIManager.TriggerStartGameUi();

        InstantiatePlayers(MultipleInputManager.NeedKeyboard, MultipleInputManager.NumberOfPlayer);

        Camera.AddTargets();
        
        StartCoroutine(RoundManager.StartRound());
    }

    public void ResumeGame()
    {
        GameState = GameState.RACING;
        UIManager.TriggerResumeGameUi();
    }
    
    public void ReturnToMenu()
    {
        GameState = GameState.PRE_GAME;
        DestroyPlayersInstance();
        UIManager.InputMenuUI.DestroyPlayersInput();
        
        SceneManager.Instance.LoadMenu();
    }

    public void LoadEndGame()
    {
        GameState = GameState.POST_GAME;
        ScoreManager.OrderPlayersAccordingToScore();
        UIManager.TriggerEndGameUi();
    }

    #endregion

    #region Game

    private void InstantiatePlayers(bool needKeyboard, int nbPlayer)
    {
        int startIndex = 0;
        int startPositionIndex = 0;
         
        //Instantiate players
        if (needKeyboard)
        {
            startIndex = 1;
            PlayersManager.CreateNewPlayer(true, startPositionIndex);
            startPositionIndex++;
        }

        for (int i = startIndex; i < nbPlayer; i++)
        {
            PlayersManager.CreateNewPlayer(false, startPositionIndex);
            startPositionIndex++;
        }
        
        ScoreManager.InitiatePlayersForCurrentMatch();
    }
    

    private void DestroyPlayersInstance()
    {
        PlayersManager.DestroyPlayers();
    }

    public void TriggerPlayerDestructionEvent(PlayerController player)
    {
        RoundManager.PlayerDiedEvent(player);
    }

    public void TriggerScoreAddEvent()
    {
        ScoreManager.AddScoreToAlivePlayers();
    }

    public void TriggerEndGameAfterRoundEvent()
    {
        RoundManager.GameFinished = true;
    }

    public void TriggerEndGameEvent()
    {
        LoadEndGame();
    }
    
    #endregion

    #region UI

    public void TriggerUiCreationForPlayerEvent(PlayerController playerInstance)
    {
        UIManager.GameUI.CreateUisForPlayer(playerInstance);
    }

    #endregion
}
