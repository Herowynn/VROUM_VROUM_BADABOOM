using DG.Tweening;
using UnityEngine;

/// <summary>
/// Menu is a script that is used on every GameObject of the Menu.
/// </summary>

public class Menu : MonoBehaviour
{
    [Header("Instance")] public CanvasGroup CanvasGroup;

    public void Load()
    {
        gameObject.SetActive(true);
    }

    public void Unload()
    {
        gameObject.SetActive(false);
    }
}
