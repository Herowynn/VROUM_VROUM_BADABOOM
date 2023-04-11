using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
