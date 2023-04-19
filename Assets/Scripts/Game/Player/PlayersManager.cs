using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayersManager is a manager that deal with the PlayerS and not a player in particular.
/// This script is here to deal with player's instantiation and personalization of their information.
/// For example, you can modify player's colors in this script.
/// </summary>

public class PlayersManager : MonoBehaviour
{
    [Header("Instances")]
    public GameObject CarPrefab;
    public GameObject PlayersContainer;
    
    [Header("Infos")]
    [HideInInspector] public List<GameObject> Players;
    public List<Color> PlayerColors;
    
    public void CreateNewPlayer(bool playerUseKeyboard, int startPositionIndex)
    {
        GameObject car = Instantiate(CarPrefab, GameManager.Instance.MapManager.CurrentMap.PlayerStartPositions[startPositionIndex].transform.position, Quaternion.identity, PlayersContainer.transform);

        if (playerUseKeyboard)
            car.GetComponent<PlayerInput>().defaultControlScheme = "Keyboard";
        else
            car.GetComponent<PlayerInput>().defaultControlScheme = "Controller";

        Players.Add(car);

        Players[^1].GetComponent<CarController>().Color = PlayerColors[startPositionIndex];
    }

    public void DestroyPlayers()
    {
        foreach (var player in Players)
        {
            Destroy(player);
        }

        Players = new List<GameObject>();
    }
}
