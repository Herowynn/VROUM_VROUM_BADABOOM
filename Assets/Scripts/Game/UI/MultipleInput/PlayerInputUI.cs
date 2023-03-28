using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputUI : MonoBehaviour
{
    [Header("Instance")] 
    public Image Status;
    
    public void ChangeStatus(Color color)
    {
        Status.color = color;
    }

    public bool IsStatusSimilar(Color color)
    {
        if (Status.color == color)
            return true;
        return false;
    }
}
