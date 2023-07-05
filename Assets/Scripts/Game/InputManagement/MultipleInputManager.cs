using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

/// <summary>
/// MultipleInputManager is the first script called when the scene loads.
/// Why ? Because it contains every GameParameters script information and is the one responsible for the initialization of the game.
/// Plus, every game starts with an InputMenu in order for the players to connect their controller or keyboard.
/// So, it also contains functions for the InputMenu.
/// </summary>

public class MultipleInputManager : MonoBehaviour
{
    [Header("Info")]
    public int NumberOfPlayer;
    public bool NeedKeyboard;
    public int NbAi;
    public string AiDifficulty;
    public string MapName;
    public int ScoreToWin;
    
    // intern var
    private int _currentStartPositionIndex;
    private int _controllersConnected;
    private int _controllerNeeded;
    private InputMenuUI _inputMenuUI;

    public void InstantiateMultipleInputManager()
    {
        NumberOfPlayer = MenuManager.Instance.NbLocal;
        NeedKeyboard = MenuManager.Instance.NeedKeyboard;
        NbAi = (MenuManager.Instance.NbAi + MenuManager.Instance.NbLocal > 4) ? (4 - MenuManager.Instance.NbLocal > 0 ? 4 - MenuManager.Instance.NbLocal : 0)
            : MenuManager.Instance.NbAi;
        AiDifficulty = MenuManager.Instance.AiDifficulty;
        MapName = MenuManager.Instance.MapName;
        ScoreToWin = MenuManager.Instance.ScoreToWin;

        _controllerNeeded = NeedKeyboard ? NumberOfPlayer - 1 : NumberOfPlayer;
        CountControllers();
        _inputMenuUI = GameManager.Instance.UIManager.InputMenuUI;
        _inputMenuUI.CreatePlayersInput(_controllerNeeded, _controllersConnected, NeedKeyboard);

        GameManager.Instance.MapManager.LoadMap(MapName);
        
        GameManager.Instance.HarvesterManager.InstantiateHarvester();
        
        GameManager.Instance.RoundManager.InitiateRoundNodesForCurrentMap();
        GameManager.Instance.RoundManager.InitiateHarvesterForCurrentMap();

        GameManager.Instance.ScoreManager.PointsToWin = ScoreToWin;

        // Subscribe to Event
        InputSystem.onDeviceChange += ListenerOnDeviceChange;
    }

    private void CountControllers()
    {
        _controllersConnected = 0;
        
        foreach (var device in InputSystem.devices)
        {
            if (device is XInputController)
                _controllersConnected++;
        }
    }

    private void DeviceAdded()
    {
        _controllersConnected++;
        _inputMenuUI.AddOkInput();
    }

    private void DeviceAddedDuringRace()
    {
        _controllersConnected++;
        Debug.Log("You can't add controllers during game");
    }

    private void DeviceRemoved()
    {
        _controllersConnected--;
        _inputMenuUI.RemoveOkInput();
    }

    private void DeviceRemovedDuringRace()
    {
        if (_controllersConnected > _controllerNeeded)
        {
            _controllersConnected--;
            return;
        }
        
        GameManager.Instance.LoadPauseGame();
        DeviceRemoved();
    }

    private void ListenerOnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                if (device is XInputController)
                {
                    if (GameManager.Instance.GameState != GameState.RACING)
                    {
                        DeviceAdded();
                    }
                    else if (GameManager.Instance.GameState == GameState.RACING)
                    {
                        DeviceAddedDuringRace();
                    }
                }
                break;

            case InputDeviceChange.Removed:
                if (device is XInputController)
                {
                    if (GameManager.Instance.GameState != GameState.RACING)
                    {
                        DeviceRemoved();
                    }
                    else if (GameManager.Instance.GameState == GameState.RACING)
                    {
                        DeviceRemovedDuringRace();
                    }
                }
                break;
        }
    }
    
    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= ListenerOnDeviceChange;
    }
}
