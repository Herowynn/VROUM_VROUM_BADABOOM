using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// UIManager is the global UI manager. His role is to display the right UI at the right time.
/// </summary>

public class UIManager : MonoBehaviour
{
    [Header("Instances")]
    public InputMenuUI InputMenuUI;
    public GameUI GameUI;
    public PostGameUI PostGameUI;
    public GameObject PauseMenu;
    public GameObject[] UisGo;

    public void DisplayInputMenu()
    {
        HideAllUIs();
        InputMenuUI.transform.gameObject.SetActive(true);
    }

    private void DisplayGameUI()
    {
        HideAllUIs();
        GameUI.gameObject.SetActive(true);
    }

    private void DisplayPostGameUI()
    {
        HideAllUIs();
        PostGameUI.gameObject.SetActive(true);
    }

    public void DisplayPauseMenu()
    {
        HideAllUIs();
        PauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        DisplayGameUI();
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
        PostGameUI.CreatePlayerPositions();
        DisplayPostGameUI();
    }

    public void OnClickRestart()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;

        SceneManager.Instance.LoadMenu();  
    }
}
