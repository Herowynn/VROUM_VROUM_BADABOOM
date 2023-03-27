using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class MultipleInputManager : MonoBehaviour
{
    [Header("Instance")] 
    public GameObject PlayerControllerPrefab;
    public GameObject PlayerKeyboardPrefab;
    public GameObject PlayerContainer;

    [Header("Info")]
    public int NumberOfPlayer;
    public bool NeedKeyboard;
    public List<GameObject> Players;

    [Header("GD")] 
    public GameObject[] StartPositions;
    
    // intern var
    private int _currentStartPositionIndex;
    private int _controllersConnected;
    private int _controllerNeeded;
    private InputMenuUI _inputMenuUI;

    
    private void Start()
    {
        _controllerNeeded = NeedKeyboard ? NumberOfPlayer - 1 : NumberOfPlayer;
        CountControllers();
        _inputMenuUI = GameManager.Instance.UIManager.InputMenuUI;
        _inputMenuUI.CreatePlayersInput(_controllerNeeded, _controllersConnected, NeedKeyboard);
        
        // Subscribe to Event
        InputSystem.onDeviceChange +=
            (device, change) =>
            {
                switch (change)
                {
                    case InputDeviceChange.Added:
                        if (device is XInputController)
                        {
                            if (GameManager.Instance.GameState == GameState.NOT_RACING)
                            {
                                DeviceAdded();
                            }
                            else if (GameManager.Instance.GameState == GameState.RACING)
                            {
                                Debug.Log("You can't add controllers during game");
                            }
                        }
                        break;

                    case InputDeviceChange.Removed:
                        if (device is XInputController)
                        {
                            if (GameManager.Instance.GameState == GameState.NOT_RACING)
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
            };
        
        /*
         int startIndex = 0;
         _currentStartPositionIndex = 0;
         
        //Instantiate players
        if (NeedKeyboard)
        {
            startIndex = 1;
            CreateNewPlayer(true);
        }

        for (int i = startIndex; i < NumberOfPlayer; i++)
        {
            CreateNewPlayer(false);
        }*/
    }

    private void CreateNewPlayer(bool playerUseKeyboard)
    {
        if (playerUseKeyboard)
        {
            Players.Add(Instantiate(PlayerKeyboardPrefab, StartPositions[_currentStartPositionIndex].transform.position, Quaternion.identity, PlayerContainer.transform));
        }
        else
        {
            Players.Add(Instantiate(PlayerControllerPrefab, StartPositions[_currentStartPositionIndex].transform.position, Quaternion.identity, PlayerContainer.transform));
        }

        _currentStartPositionIndex++;
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

    private void DeviceRemoved()
    {
        _controllersConnected--;
        _inputMenuUI.RemoveOkInput();
    }

    private void DeviceRemovedDuringRace()
    {
        GameManager.Instance.UIManager.DisplayInputMenu();
        GameManager.Instance.GameState = GameState.NOT_RACING;//will trigger pause menu
        DeviceRemoved();
    }
}
