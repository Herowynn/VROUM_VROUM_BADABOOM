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
    public GameObject PlayerControllerPrefab;
    public GameObject PlayerKeyboardPrefab;
    public GameObject PlayerContainer;
    public CameraController Camera;
    public ScoreManager ScoreManager;
    public RoundManager RoundManager;
    public MapManager MapManager;

    [Header("Info")]
    public GameState GameState;
    public List<GameObject> Players;
    public GameObject[] StartPositions;

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
            CreateNewPlayer(true, startPositionIndex);
            startPositionIndex++;
        }

        for (int i = startIndex; i < nbPlayer; i++)
        {
            CreateNewPlayer(false, startPositionIndex);
            startPositionIndex++;
        }
    }
    
    private void CreateNewPlayer(bool playerUseKeyboard, int startPositionIndex)
    {
        if (playerUseKeyboard)
        {
            Players.Add(Instantiate(PlayerKeyboardPrefab, MapManager.CurrentMap.StartPositions[startPositionIndex].transform.position, Quaternion.identity, PlayerContainer.transform));
        }
        else
        {
            Players.Add(Instantiate(PlayerControllerPrefab, MapManager.CurrentMap.StartPositions[startPositionIndex].transform.position, Quaternion.identity, PlayerContainer.transform));
        }
    }

    private void DestroyPlayersInstance()
    {
        foreach (var player in Players)
        {
            Destroy(player);
        }

        Players = new List<GameObject>();
    }

    public void TriggerPlayerDestructionEvent(GameObject playerGo)
    {
        RoundManager.PlayerDiedEvent(playerGo);
    }
        
    #endregion
}
