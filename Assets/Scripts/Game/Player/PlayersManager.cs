using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public GameObject[] RankedPlayers;


    private float _carsWeightedSpeed;
    private Dictionary<int, float[]> _weights = new Dictionary<int, float[]>();

    public float CarsWeightedSpeed { get { return _carsWeightedSpeed; } }

    private void Start()
    {
        _weights[2] = new float[] { 0.9f, 0.1f };
        _weights[3] = new float[] { 0.9f, 0.06f, 0.04f };
        _weights[4] = new float[] { 0.9f, 0.04f, 0.03f, 0.03f };
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING)
        {
            CarsOnScreenVerification();

            RankedPlayers = GameManager.Instance.RoundManager.RealtimeCarsRanking(Camera.main.GetComponent<CameraController>().Targets);

            if (RankedPlayers.Length > 1)
            {
                List<float> rankedSpeed = new List<float>();

                foreach (GameObject player in RankedPlayers)
                    rankedSpeed.Add(player.GetComponent<GlobalController>().SphereRB.velocity.magnitude);

                rankedSpeed.Sort();

                float weightedSpeed = 0;

                for (int i = 0; i < rankedSpeed.Count; i++)
                    weightedSpeed += rankedSpeed[rankedSpeed.Count - 1 - i] * _weights[RankedPlayers.Length][i];

                _carsWeightedSpeed = weightedSpeed;
            }
        }
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
        foreach (GameObject car in Players)
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

                if (visibleParts == 0 && GameManager.Instance.GameState == GameState.RACING)
                    GameManager.Instance.TriggerPlayerDestructionEvent(gc);
            }
        }
    }
}
