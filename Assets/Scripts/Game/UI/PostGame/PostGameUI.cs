using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PostGameUI is used for the PostGame and his role is to manage it.
/// It includes some personalization elements such as the Position's name.
/// Example : First or 1st or Ist...
/// </summary>

public class PostGameUI : MonoBehaviour
{
    [Header("Instance")]
    public PlayerPosUI[] PlayerPosUis;
    
    [Header("GA")] 
    public string[] PlayerPositionsName;

    public void CreatePlayerPositions()
    {
        // Hide unnecessary positions
        for (int i = PlayerPosUis.Length - 1; i > GameManager.Instance.PlayersManager.Players.Count - 1; i--)
        {
            PlayerPosUis[i].gameObject.SetActive(false);
        }

        List<CarController> playerControllersSorted = GameManager.Instance.ScoreManager.Players;

        for (int i = 0; i < playerControllersSorted.Count; i++)
        {
            PlayerPosUis[i].ChangePlayerColor(playerControllersSorted[i].Color);
            PlayerPosUis[i].ChangePositionText(PlayerPositionsName[i]);
        }
    }
}
