using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("GA")] 
    public string MenuTitle;
    public string MenuFooter;

    public void Load()
    {
        transform.gameObject.SetActive(true);
    }

    public void Unload()
    {
        transform.gameObject.SetActive(false);
    }
}
