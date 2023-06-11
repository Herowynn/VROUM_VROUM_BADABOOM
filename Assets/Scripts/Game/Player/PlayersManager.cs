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
    public GameObject HumanPlayerPrefab;
    public GameObject AiPlayerPrefab;
    public GameObject PlayersContainer;
    
    [Header("Infos")]
    public List<GameObject> Players;
    public List<Color> PlayerColors;
    
    public void CreateNewPlayer(bool playerUseKeyboard, int startPositionIndex, bool isAi)
    {
        Vector3 spawnPosition = GameManager.Instance.MapManager.CurrentMap.PlayerStartPositions[startPositionIndex].transform.position;
        GameObject car = Instantiate(isAi ? AiPlayerPrefab : HumanPlayerPrefab, spawnPosition, Quaternion.identity, PlayersContainer.transform);

        car.transform.rotation = GameManager.Instance.MapManager.CurrentMap.PlayerStartPositions[startPositionIndex]
            .transform.rotation;
        
        if (playerUseKeyboard && !isAi)
            car.GetComponent<PlayerInput>().defaultControlScheme = "Keyboard";
        else if (!isAi)
            car.GetComponent<PlayerInput>().defaultControlScheme = "Controller";

        Players.Add(car);

        Players[^1].GetComponent<GlobalController>().Color = PlayerColors[startPositionIndex];
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
