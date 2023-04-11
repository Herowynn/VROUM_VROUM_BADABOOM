using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Instances")] 
    public GameObject PointsUiPrefab;
    public GameObject ProfilePrefab;
    public GameObject PointsContainer;
    public GameObject ProfileContainer;

    public void CreateUisForPlayer(PlayerController playerInstance)
    {
        GameObject pointsGo = Instantiate(PointsUiPrefab, PointsContainer.transform);
        GameObject profileGo = Instantiate(ProfilePrefab, ProfileContainer.transform);

        playerInstance.PointsUI = pointsGo.GetComponent<PointsUI>();
        playerInstance.ProfileUI = profileGo.GetComponent<ProfileUI>();
    }
}
