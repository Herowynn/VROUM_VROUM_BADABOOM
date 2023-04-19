using UnityEngine;

/// <summary>
/// GameUI acts as a little UIManager but only for the In-Game UI that displays the points
/// and the bonus the car has. This script is also responsible for the creation of each players UIs.
/// </summary>

public class GameUI : MonoBehaviour
{
    [Header("Instances")] 
    public GameObject PointsUiPrefab;
    public GameObject ProfilePrefab;
    public GameObject PointsContainer;
    public GameObject ProfileContainer;

    public void CreateUisForPlayer(CarController playerInstance)
    {
        GameObject pointsGo = Instantiate(PointsUiPrefab, PointsContainer.transform);
        GameObject profileGo = Instantiate(ProfilePrefab, ProfileContainer.transform);

        playerInstance.PointsUI = pointsGo.GetComponent<PointsUI>();
        playerInstance.ProfileUI = profileGo.GetComponent<ProfileUI>();
    }
}
