using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Instances")]
    public InputMenuUI InputMenuUI;
    public GameUI GameUI;
    public GameObject[] UIsGo;

    public void DisplayInputMenu()
    {
        HideAllUIs();
        InputMenuUI.transform.gameObject.SetActive(true);
    }

    public void DisplayGameUI()
    {
        HideAllUIs();
        GameUI.transform.gameObject.SetActive(true);
    }

    private void HideAllUIs()
    {
        foreach (var ui in UIsGo)
        {
            ui.SetActive(false);
        }
    }
    
}
