using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [Header("Instances")] 
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Footer;
    public Menu[] Menus;
    public Menu FirstMenuLoaded;
    public GameParameters GameParameters;

    [Header("Game Parameters Informations")] 
    public int NbLocal;
    public int NbAi;
    public string AiDifficulty;
    public string MapName;
    public int ScoreToWin;

    [Header("Game Parameters possibilities")]
    public int[] PossibleLocalPlayers;
    public int[] PossibleAiPlayers;
    public string[] PossibleAiDifficulties;
    public string[] PossibleMaps;
    public int[] PossibleScores;

    private Menu _currentMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadMenu(FirstMenuLoaded);
    }
    
    public void LoadMenu(Menu menu)
    {
        if (_currentMenu != null)
            _currentMenu.Unload();

        _currentMenu = menu;
        _currentMenu.Load();
        Title.text = _currentMenu.MenuTitle;
        Footer.text = _currentMenu.MenuFooter;
    }
    
    public void QuitGame() {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    #region Game Parameters

    public void SetDynamicDropdowns()
    {
        List<string> options = new List<string>();
        for (int i = 0; i < PossibleLocalPlayers.Length; i++)
        {
            options.Add(PossibleLocalPlayers[i].ToString());
        }
        GameParameters.NbLocal.AddOptions(options);
        
        options = new List<string>();
        for (int i = 0; i < PossibleAiPlayers.Length; i++)
        {
            options.Add(PossibleAiPlayers[i].ToString());
        }
        GameParameters.NbAi.AddOptions(options);
        
        options = new List<string>();
        for (int i = 0; i < PossibleAiDifficulties.Length; i++)
        {
            options.Add(PossibleAiDifficulties[i]);
        }
        GameParameters.AiDifficulty.AddOptions(options);
        
        options = new List<string>();
        for (int i = 0; i < PossibleMaps.Length; i++)
        {
            options.Add(PossibleMaps[i]);
        }
        GameParameters.MapSelection.AddOptions(options);
        
        options = new List<string>();
        for (int i = 0; i < PossibleScores.Length; i++)
        {
            options.Add(PossibleScores[i].ToString());
        }
        GameParameters.ScoreToWin.AddOptions(options);
    }

    public void UpdateNbLocal(int index)
    {
        NbLocal = PossibleLocalPlayers[index];
    }
    public void UpdateNbAi(int index)
    {
        NbAi = PossibleAiPlayers[index];
    }

    public void UpdateAiDifficulty(int index)
    {
        AiDifficulty = PossibleAiDifficulties[index];
    }

    public void UpdateMap(int index)
    {
        MapName = PossibleMaps[index];
    }

    public void UpdateScoreToWin(int index)
    {
        ScoreToWin = PossibleScores[index];
    }

    #endregion
}
