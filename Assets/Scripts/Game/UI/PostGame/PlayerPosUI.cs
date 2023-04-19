using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerPosUI is a script used in the Post Game UI.
/// It is related to the prefab PlayerPos that displays the color of the player + his ranking position after the game.
/// </summary>

public class PlayerPosUI : MonoBehaviour
{
    [Header("Instance")] 
    public Image PlayerVisual;

    public TextMeshProUGUI Position;

    public void ChangePlayerColor(Color color)
    {
        PlayerVisual.color = color;
    }

    public void ChangePositionText(string pos)
    {
        Position.text = pos;
    }
}
