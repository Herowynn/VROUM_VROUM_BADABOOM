using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// MenuManager is a singleton that is used to manage every aspect of the Menu scene.
/// It is used as the main referencer for the Header and the Footer for example, but also contains a list of every menu accessible,
/// the game parameters possibilities and most importantly, the variables to load the Game Scene.
/// </summary>

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        foreach (var menu in _menus)
        {
            menu.Load();
        }
    }

    [Header("Instances")] [SerializeField] private Menu _firstMenuToLoad;
    [SerializeField] private Menu _mainMenu;
    [SerializeField] private GameParameters _gameParameters;
    [SerializeField] private Options _options;
    [SerializeField] private Menu[] _menus;

    [Header("Game Parameters Informations")] 
    public int NbLocal;
    public bool NeedKeyboard;
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

    #region private Fields
    private Menu _currentMenu;
    #endregion

    private void Start()
    {
        // Init
        FillAllDropdowns();
        SetQuality(0);
        
        // Finish init
        foreach (var menu in _menus)
        {
            menu.Unload();
        }
        
        LoadMenu(_firstMenuToLoad);
        
        AudioManager.Instance.PlayMusicAndLoop("BlazeRushMainMenuMusic");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _firstMenuToLoad.gameObject.activeSelf)
        {
            LoadMenu(_mainMenu);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _mainMenu.gameObject.activeSelf)
        {
            LoadMenu(_firstMenuToLoad);
        }
    }

    /// <summary>
    /// Function used to load each menu window
    /// </summary>
    /// <param name="menu"></param>
    public void LoadMenu(Menu menu)
    {
        if (_currentMenu != null)
        {
            _currentMenu.CanvasGroup.DOKill();
            
            _currentMenu.CanvasGroup.alpha = 1f;
            
            _currentMenu.CanvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
            {
                _currentMenu.Unload();
                
                _currentMenu = menu;
                _currentMenu.CanvasGroup.alpha = 0f;
                
                _currentMenu.Load();

                _currentMenu.CanvasGroup.DOFade(1f, 0.5f);
            });
        }

        else
        {
            _currentMenu = menu;
            _currentMenu.CanvasGroup.alpha = 0f;
            
            _currentMenu.Load();

            _currentMenu.CanvasGroup.DOFade(1f, 0.5f);
        }
    }
    
    
    public void LoadGame()
    {
        _currentMenu.CanvasGroup.DOKill();
            
        _currentMenu.CanvasGroup.alpha = 1f;
            
        _currentMenu.CanvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {
            _currentMenu.Unload();
                
            _currentMenu = _firstMenuToLoad;
            _currentMenu.CanvasGroup.alpha = 1f;
                
            _currentMenu.Load();
            
            SceneManager.Instance.LoadGame();
        });
        
        
    }
    
    #region Main Menu

    public void QuitGame() {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    #endregion
    
    #region Game Parameters

    #region Dynamism
    private void FillDropdownWithIntTab(int[] intTab, TMP_Dropdown dropdownToFill)
    {
        List<string> options = new List<string>();
        for (int i = 0; i < intTab.Length; i++)
        {
            options.Add(intTab[i].ToString());
        }
        dropdownToFill.AddOptions(options);
    }

    private void FillDropdownWithStringTab(string[] strTab, TMP_Dropdown dropdownToFill)
    {
        List<string> options = new List<string>();
        for (int i = 0; i < strTab.Length; i++)
        {
            options.Add(strTab[i]);
        }
        dropdownToFill.AddOptions(options);
    }
    
    private void FillAllDropdowns()
    {
        // Nb Local
        FillDropdownWithIntTab(PossibleLocalPlayers, _gameParameters.NbLocal);

        // Nb AI
        FillDropdownWithIntTab(PossibleAiPlayers, _gameParameters.NbAi);
        
        // Ai difficulty
        FillDropdownWithStringTab(PossibleAiDifficulties, _gameParameters.AiDifficulty);
        
        // Maps
        FillDropdownWithStringTab(PossibleMaps, _gameParameters.MapSelection);

        // Score to win
        FillDropdownWithIntTab(PossibleScores, _gameParameters.ScoreToWin);
    }
    #endregion

    #region Listeners
    public void SetNbLocalEvent(int localIndex)
    {
        NbLocal = PossibleLocalPlayers[localIndex];
    }
    public void SetNbAiEvent(int aiIndex)
    {
        NbAi = PossibleAiPlayers[aiIndex];
    }
    public void SetAiDifficultyEvent(int difficultyIndex)
    {
        AiDifficulty = PossibleAiDifficulties[difficultyIndex];
    }
    public void SetMapEvent(int mapIndex)
    {
        MapName = PossibleMaps[mapIndex];
    }
    public void SetScoreToWinEvent(int scoreIndex)
    {
        ScoreToWin = PossibleScores[scoreIndex];
    }
    public void ToggleKeyboardNeed(bool needKeyboardStatus)
    {
        NeedKeyboard = needKeyboardStatus;
    }
    #endregion

    #endregion

    #region Options
    public void SetGlobalVolume(float volume) 
    {
        _options.AudioMixer.SetFloat("Main_Volume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        _options.AudioMixer.SetFloat("SFX_Volume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        _options.AudioMixer.SetFloat("MUSIC_Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _options.Resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    #endregion
    
    #region Sounds
    
    public void PlaySoundEffectForParameterChange()
    {
        AudioManager.Instance.PlaySfx("ValidateParameterSoundEffect");
    }

    public void PlaySoundEffectWhenNavButtonClicked()
    {
        AudioManager.Instance.PlaySfx("NavBtnClickedSoundEffect");
    }

    public void PlaySoundEffectForBackButtonClicked()
    {
        AudioManager.Instance.PlaySfx("BackSoundEffect");
    }

    public void PlaySoundEffectForConfirmButtonClicked()
    {
        AudioManager.Instance.PlaySfx("ConfirmSoundEffect");
    }
    
    #endregion
}
