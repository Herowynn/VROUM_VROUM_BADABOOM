using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Instances")] 
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Footer;
    public Menu[] Menus;
    public Menu FirstMenuLoaded;
    
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
}
