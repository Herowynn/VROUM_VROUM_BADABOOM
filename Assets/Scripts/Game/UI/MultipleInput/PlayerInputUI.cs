using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerInputUI is used in every PlayerInputPrefab.
/// His role is to modify the prefab according to the Input state.
/// For example, it turns red if an input is not connected.
/// </summary>

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
