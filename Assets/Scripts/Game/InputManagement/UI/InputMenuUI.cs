using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMenuUI : MonoBehaviour
{
    [Header("Instance")] 
    public Color GoodStatus;
    public Color BadStatus;
    public GameObject PlayerInputPrefab;
    public GameObject PlayerInputsContainer;
    public GameObject StartButton;

    private List<PlayerInputUI> _playerInputUIs = new List<PlayerInputUI>();

    private void CreateOnePlayerInput(int index, bool isInputValid)
    {
        GameObject go = Instantiate(PlayerInputPrefab, PlayerInputsContainer.transform);
        Debug.Log(go);
        
        PlayerInputUI playerInputUI = go.GetComponent<PlayerInputUI>();
        _playerInputUIs.Add(playerInputUI);
        
        
        
        playerInputUI.ChangeStatus(isInputValid ? GoodStatus : BadStatus);
    }

    public void CreatePlayersInput(int totalPlayerInput, int nbPlayerInputValid)
    {
        int cpt = 0;

        for (int i = 1; i <= totalPlayerInput; i++)
        {
            CreateOnePlayerInput(i, cpt < nbPlayerInputValid);
            cpt++;
        }
    }

    public void AddOkInput()
    {
        for (int i = 0; i < _playerInputUIs.Count; i++)
        {
            if (_playerInputUIs[i].IsStatusSimilar(BadStatus))
            {
                _playerInputUIs[i].ChangeStatus(GoodStatus);
                break;
            }
        }
        
        CheckAllStatus();
    }

    public void RemoveOkInput()
    {
        for (int i = _playerInputUIs.Count - 1; i >= 0; i--)
        {
            if (_playerInputUIs[i].IsStatusSimilar(GoodStatus))
            {
                _playerInputUIs[i].ChangeStatus(BadStatus);
                break;
            }
        }
        
        CheckAllStatus();
    }

    private void CheckAllStatus()
    {
        foreach (var ui in _playerInputUIs)
        {
            if (ui.IsStatusSimilar(BadStatus))
            {
                StartButton.SetActive(false);
                return;
            }
        }
        
        StartButton.SetActive(true);
    }
}
