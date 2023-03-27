using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
	}
    
    [Header("Names")]
    public string Menu;
    public string Game;

    public void LoadMenu()
    {
	    LoadScene(Menu);
    }

    public void LoadGame()
    {
	    LoadScene(Game);
    }
    
    private void LoadScene(string scName)
    {
	    UnityEngine.SceneManagement.SceneManager.LoadScene(scName);
    }
}
