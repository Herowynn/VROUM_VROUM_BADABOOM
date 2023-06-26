using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows.Speech;

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


    private float _weightedCarsSpeed;
    private Dictionary<int, float[]> weights = new Dictionary<int, float[]>();

    public float WeightedCarsSpeed { get { return _weightedCarsSpeed; } }

    private void Start()
    {
        weights[2] = new float[] { 0.9f, 0.1f };
        weights[3] = new float[] { 0.9f, 0.06f, 0.04f };
        weights[4] = new float[] { 0.9f, 0.04f, 0.03f, 0.03f };
    }

    private void Update()
    {
/*        if (GameManager.Instance.GameState == GameState.RACING)
            CarsOnScreenVerification();*/

        float speedSum = 0;
        int cpt = 0;
        foreach(GameObject player in Players)
        {
            if (player.GetComponent<GlobalController>().PlayerState == PlayerState.ALIVE)
            {
                speedSum += player.GetComponent<GlobalController>().SphereRB.velocity.magnitude *
                    weights[Camera.main.GetComponent<CameraController>().Targets.Count][cpt];
                cpt++;
            }
        }
        _weightedCarsSpeed = speedSum;
    }

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
            Destroy(player);

        Players = new List<GameObject>();
    }

    private void CarsOnScreenVerification()
    {
        foreach(GameObject car in Players)
        {
            if (car.GetComponent<GlobalController>().PlayerState == PlayerState.ALIVE)
            {
                GlobalController gc = car.GetComponent<GlobalController>();
                int visibleParts = 0;

                foreach (GameObject carPart in gc.VisibleCarParts)
                {
                    if (carPart.GetComponent<Renderer>().isVisible)
                        visibleParts++;
                }

                if (visibleParts == 0)
                    GameManager.Instance.TriggerPlayerDestructionEvent(gc);
            }
        }
    }
}
