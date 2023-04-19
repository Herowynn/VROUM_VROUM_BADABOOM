using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PointsUI is used in every PointsPrefab to get the references and functions to modify the visual.
/// The points prefabs are used to display the color of the player + his points.
/// </summary>

public class PointsUI : MonoBehaviour
{
    [Header("Instance")] 
    public Image Visual;
    public TextMeshProUGUI Points;

    public void ChangePointsCount(int count)
    {
        Points.text = count.ToString();
    }

    public void ChangeVisualColoration(Color color)
    {
        Visual.color = color;
    }
}
