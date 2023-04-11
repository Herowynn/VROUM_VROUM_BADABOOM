using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Instances")]
    public InputMenuUI InputMenuUI;
    public GameUI GameUI;
    public GameObject[] UisGo;

    public void DisplayInputMenu()
    {
        HideAllUIs();
        InputMenuUI.transform.gameObject.SetActive(true);
    }

    private void DisplayGameUI()
    {
        HideAllUIs();
        GameUI.transform.gameObject.SetActive(true);
    }

    private void HideAllUIs()
    {
        foreach (var ui in UisGo)
        {
            ui.SetActive(false);
        }
    }

    public void TriggerStartGameUi()
    {
        InputMenuUI.StartButton.SetActive(false);
        DisplayGameUI();
    }

    public void TriggerResumeGameUi()
    {
        InputMenuUI.ResumeButton.SetActive(false);
        DisplayGameUI();
    }

    public void TriggerEndGameUi()
    {
        Debug.Log("End game ui");
    }
}
