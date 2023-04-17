using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Fields

    [Header("Instance")]
    public MultipleInputManager MultipleInputManager;
    public UIManager UIManager;
    public CameraController Camera;
    public ScoreManager ScoreManager;
    public RoundManager RoundManager;
    public MapManager MapManager;
    public PlayersManager PlayersManager;
    public BonusManager BonusManager;

    [Header("Info")]
    public GameState GameState;

    [Header("Breakable Objects Layers")]
    public int CarLayerNumber;
    public int BonusLayerNumber;

    #endregion

    #region Private Fields

    private GameObject _checkPointsParentObject;
    private GameObject _harvester;
    private List<Transform> _checkPointsList = new List<Transform>();
    private int _nextCheckPointIndex;
    private Vector3 _direction;
    private Vector3 _realPointToReach;

    #endregion

    public int NextCheckpointIndex { get { return _nextCheckPointIndex; } set { _nextCheckPointIndex = value; } }

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        //LoadPreGame();
        //test
        StartGame();

        for (int i = 0; i < MapManager.CurrentMap.transform.childCount; i++)
        {
            if (MapManager.CurrentMap.transform.GetChild(i).gameObject.name == "Checkpoints")
                _checkPointsParentObject = MapManager.CurrentMap.transform.GetChild(i).gameObject;
            if (MapManager.CurrentMap.transform.GetChild(i).gameObject.name == "Harvester")
                _harvester = MapManager.CurrentMap.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < _checkPointsParentObject.transform.childCount; i++)
        {
            _checkPointsList.Add(_checkPointsParentObject.transform.GetChild(i));
        }

        _nextCheckPointIndex = 0;
    }

    void Update()
    {
        if (_nextCheckPointIndex >= _checkPointsParentObject.transform.childCount)
            _nextCheckPointIndex = 0;

        _realPointToReach = new Vector3(_checkPointsList[_nextCheckPointIndex].position.x, _harvester.transform.position.y, _checkPointsList[_nextCheckPointIndex].position.z);
        _direction = (_realPointToReach - _harvester.transform.position).normalized;
        _harvester.GetComponent<Harvester>().direction = _direction;
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

        MultipleInputManager.InstantiateMultipleInputManager();

        InstantiatePlayers(MultipleInputManager.NeedKeyboard, MultipleInputManager.NumberOfPlayer + MultipleInputManager.NbAi);

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

    public void TriggerPlayerDestructionEvent(CarController player)
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

    public void DestroyBonusEvent()
    {
        BonusManager.ClearBonus();
    }
    
    #endregion

    #region UI

    public void TriggerUiCreationForPlayerEvent(CarController playerInstance)
    {
        UIManager.GameUI.CreateUisForPlayer(playerInstance);
    }

    #endregion
}
