using System.Collections;
using UnityEngine;

/// <summary>
/// GameManager is a singleton that is used for the global behavior of the game.
/// It contains every necessary managers for the game to run.
/// It also contains functions that trigger manager's event and that are called by other managers.
/// </summary>

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
    public HarvesterManager HarvesterManager;

    [Header("Info")]
    public GameState GameState;

    [Header("FX")]
    public float OnScreenExplosionDuration;
    public GameObject ExplosionEffectObject;
    
    #endregion
    
    //Singleton
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
    }

    #region Game State

    public void LoadPreGame()
    {
        GameState = GameState.PRE_GAME;
        UIManager.DisplayInputMenu();
        MultipleInputManager.InstantiateMultipleInputManager();
    }

    public void LoadPauseGame()
    {
        GameState = GameState.NOT_RACING;
        UIManager.DisplayInputMenu();
    }
    
    public void StartGame()
    {
        UIManager.TriggerStartGameUi();

        //Test
        MultipleInputManager.InstantiateMultipleInputManager();

        InstantiatePlayers(MultipleInputManager.NeedKeyboard, MultipleInputManager.NumberOfPlayer, MultipleInputManager.NbAi);

        Camera.InitiatePosition();
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

    private void InstantiatePlayers(bool needKeyboard, int nbPlayer, int nbAi)
    {
        int startIndex = 0;
        int startPositionIndex = 0;
         
        //Instantiate players
        if (needKeyboard)
        {
            startIndex = 1;
            PlayersManager.CreateNewPlayer(true, startPositionIndex, false);
            startPositionIndex++;
        }

        for (int i = startIndex; i < nbPlayer; i++)
        {
            PlayersManager.CreateNewPlayer(false, startPositionIndex, false);
            startPositionIndex++;
        }

        for (int i = 0; i < nbAi; i++)
        {
            PlayersManager.CreateNewPlayer(false, startPositionIndex, true);
            startPositionIndex++;
        }
        
        ScoreManager.InitiatePlayersForCurrentMatch();
    }
    
    private void DestroyPlayersInstance()
    {
        PlayersManager.DestroyPlayers();
    }

    private IEnumerator CarExplosion(Vector3 explosionCenter)
    {
        GameObject carExplosionObject = Instantiate(ExplosionEffectObject, explosionCenter, Quaternion.identity);
        carExplosionObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(OnScreenExplosionDuration);
        Destroy(carExplosionObject);
    }

    public void TriggerPlayerDestructionEvent(GlobalController player)
    {
        StartCoroutine(CarExplosion(player.SphereRB.transform.position));
        RoundManager.PlayerDiedEvent(player);
        Camera.RemoveDeadTargetEvent();
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

    public void TriggerUiCreationForPlayerEvent(GlobalController playerInstance)
    {
        UIManager.GameUI.CreateUisForPlayer(playerInstance);
    }

    #endregion
}
