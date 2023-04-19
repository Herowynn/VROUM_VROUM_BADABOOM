using UnityEngine;

/// <summary>
/// Menu is a script that is used on every GameObject of the Menu.
/// It allows to personalize the Header and the Footer for every menu, since it's never the same.
/// </summary>

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
