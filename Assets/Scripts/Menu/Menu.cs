using UnityEngine;

/// <summary>
/// Menu is a script that is used on every GameObject of the Menu.
/// </summary>

public class Menu : MonoBehaviour
{
    public void Load()
    {
        gameObject.SetActive(true);
    }

    public void Unload()
    {
        gameObject.SetActive(false);
    }
}
