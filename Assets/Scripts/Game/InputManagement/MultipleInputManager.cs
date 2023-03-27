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
    public InputMenuUI InputMenuUI;

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
    
    private void Start()
    {
        int startIndex = 0;
        _currentStartPositionIndex = 0;

        _controllerNeeded = NeedKeyboard ? NumberOfPlayer - 1 : NumberOfPlayer;
        
        CountControllers();
        
        InputMenuUI.CreatePlayersInput(_controllerNeeded, _controllersConnected);
        
        // Subscribe to Event
        InputSystem.onDeviceChange +=
            (device, change) =>
            {
                switch (change)
                {
                    case InputDeviceChange.Added:
                        if (device is XInputController)
                        {
                            _controllersConnected++;
                            InputMenuUI.AddOkInput();
                        }
                        break;

                    case InputDeviceChange.Removed:
                        if (device is XInputController)
                        {
                            _controllersConnected--;
                            InputMenuUI.RemoveOkInput();
                        }
                        break;
                }
            };
        
        /*
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

    private void Update()
    {
        if (_controllersConnected < _controllerNeeded)
            Debug.Log("You need to connect more controllers");
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
        
        Debug.Log(_controllersConnected);
    }
}
