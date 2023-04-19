using UnityEngine;

/// <summary>
/// This class manages the instantiation of the harvester.
/// </summary>
public class HarvesterManager : MonoBehaviour
{
    [Header("Instances")] 
    public GameObject HarvesterPrefab;
    
    [HideInInspector] public GameObject HarvesterGoRef;
    [HideInInspector] public Harvester HarvesterRef;

    /// <summary>
    /// This method instantiates the harvester prefab, sets its scale, its rotation and calls the InitiateNodesToFollow 
    /// method that initialize harvester path.
    /// </summary>
    public void InstantiateHarvester()
    {
        HarvesterGoRef = Instantiate(HarvesterPrefab, GameManager.Instance.MapManager.CurrentMap.HarvesterStartPosition.position, Quaternion.identity, transform);

        //Scale fix
        HarvesterGoRef.transform.localScale = GameManager.Instance.MapManager.CurrentMap.gameObject.transform.localScale;
        HarvesterGoRef.transform.rotation = GameManager.Instance.MapManager.CurrentMap.HarvesterStartPosition.rotation;
        
        HarvesterRef = HarvesterGoRef.GetComponent<Harvester>();
        HarvesterRef.InitiateNodesToFollow(GameManager.Instance.MapManager.CurrentMap.HarvesterNodes);
    }
}
