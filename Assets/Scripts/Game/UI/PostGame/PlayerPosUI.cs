using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
