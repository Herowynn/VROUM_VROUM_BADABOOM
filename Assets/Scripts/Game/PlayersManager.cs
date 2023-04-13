using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    [Header("Instances")] 
    public GameObject PlayerKeyboardPrefab;
    public GameObject PlayerControllerPrefab;
    public GameObject PlayersContainer;
    
    [Header("Infos")]
    public List<GameObject> Players;
    public List<Color> PlayerColors;
    
    //intern var

    public void CreateNewPlayer(bool playerUseKeyboard, int startPositionIndex)
    {
        if (playerUseKeyboard)
        {
            Players.Add(Instantiate(PlayerKeyboardPrefab, GameManager.Instance.MapManager.CurrentMap.StartPositions[startPositionIndex].transform.position, Quaternion.identity, PlayersContainer.transform));
        }
        else
        {
            Players.Add(Instantiate(PlayerControllerPrefab, GameManager.Instance.MapManager.CurrentMap.StartPositions[startPositionIndex].transform.position, Quaternion.identity, PlayersContainer.transform));
        }

        Players[^1].GetComponent<PlayerController>().Color = PlayerColors[startPositionIndex];
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
