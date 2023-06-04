using UnityEngine;

/// <summary>
/// UIManager is the global UI manager. His role is to display the right UI at the right time.
/// </summary>

public class UIManager : MonoBehaviour
{
    [Header("Instances")]
    public InputMenuUI InputMenuUI;
    public GameUI GameUI;
    public PostGameUI PostGameUI;
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

    private void DisplayPostGameUI()
    {
        HideAllUIs();
        PostGameUI.transform.gameObject.SetActive(true);
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
        SceneManager.Instance.LoadMenu();  
    }
}
