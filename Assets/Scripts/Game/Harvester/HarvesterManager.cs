using UnityEngine;

public class HarvesterManager : MonoBehaviour
{
    [Header("Instances")] 
    public GameObject HarvesterPrefab;
    
    [HideInInspector] public GameObject HarvesterGoRef;
    [HideInInspector] public Harvester HarvesterRef;


    public void InstantiateHarvester()
    {
        HarvesterGoRef = Instantiate(HarvesterPrefab, GameManager.Instance.MapManager.CurrentMap.HarvesterStartPosition.position,
            Quaternion.identity, transform);

        //Scale fix
        HarvesterGoRef.transform.localScale =
            GameManager.Instance.MapManager.CurrentMap.gameObject.transform.localScale;
        
        HarvesterRef = HarvesterGoRef.GetComponent<Harvester>();
        HarvesterRef.InitiateNodesToFollow(GameManager.Instance.MapManager.CurrentMap.HarvesterNodes);
    }
}
