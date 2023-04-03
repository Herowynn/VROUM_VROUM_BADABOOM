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
        
        StartGame();
    }

    [Header("Instance")]
    public GameState GameState;
    public MultipleInputManager MultipleInputManager;
    public UIManager UIManager;
    public GameObject PlayerControllerPrefab;
    public GameObject PlayerKeyboardPrefab;
    public GameObject PlayerContainer;

    [Header("Info")]
    public List<GameObject> Players;
    public GameObject[] StartPositions;

    private void Start()
    {
        //LoadPreGame();
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
        GameState = GameState.RACING;
        UIManager.TriggerStartGameUi();
        
        InstantiatePlayers(MultipleInputManager.NeedKeyboard, MultipleInputManager.NumberOfPlayer);
    }

    public void ResumeGame()
    {
        GameState = GameState.RACING;
        UIManager.TriggerResumeGameUi();
    }
    
    public void ReturnToMenu()
    {
        GameState = GameState.PRE_GAME;
        DestroyPlayers();
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
            Players.Add(Instantiate(PlayerKeyboardPrefab, StartPositions[startPositionIndex].transform.position, Quaternion.identity, PlayerContainer.transform));
        }
        else
        {
            Players.Add(Instantiate(PlayerControllerPrefab, StartPositions[startPositionIndex].transform.position, Quaternion.identity, PlayerContainer.transform));
        }
    }

    private void DestroyPlayers()
    {
        foreach (var player in Players)
        {
            Destroy(player);
        }

        Players = new List<GameObject>();
    }
        
    #endregion
}