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
    public GameObject ResumeButton;

    private List<PlayerInputUI> _playerInputUIs = new List<PlayerInputUI>();

    private void CreateOnePlayerInput(int index, bool isInputValid)
    {
        GameObject go = Instantiate(PlayerInputPrefab, PlayerInputsContainer.transform);

        PlayerInputUI playerInputUI = go.GetComponent<PlayerInputUI>();
        _playerInputUIs.Add(playerInputUI);
        
        playerInputUI.ChangeStatus(isInputValid ? GoodStatus : BadStatus);
    }

    public void CreatePlayersInput(int totalPlayerInput, int nbPlayerInputValid, bool keyboardPlayer)
    {
        int cpt = 0;
        
        if (keyboardPlayer)
            CreateOnePlayerInput(0, true);

        for (int i = 1; i <= totalPlayerInput; i++)
        {
            CreateOnePlayerInput(i, cpt < nbPlayerInputValid);
            cpt++;
        }
        
        CheckAllStatus();
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
        GameObject buttonToSet = GameManager.Instance.GameState == GameState.PRE_GAME ? StartButton : ResumeButton;
        
        foreach (var ui in _playerInputUIs)
        {
            if (ui.IsStatusSimilar(BadStatus))
            {
                buttonToSet.SetActive(false);
                return;
            }
        }
        
        buttonToSet.SetActive(true);
    }

    public void DestroyPlayersInput()
    {
        foreach (var ui in _playerInputUIs)
        {
            Destroy(ui.transform.gameObject);
        }

        _playerInputUIs = new List<PlayerInputUI>();
    }
}
