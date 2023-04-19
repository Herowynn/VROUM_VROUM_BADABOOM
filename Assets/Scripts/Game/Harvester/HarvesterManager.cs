using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages the instantiation of the harvester.
/// </summary>
public class HarvesterManager : MonoBehaviour
{
    [Header("Instances")]
    public List<HomemadeStringGameObjectPair> Harvesters = new List<HomemadeStringGameObjectPair>();

    [HideInInspector] public GameObject HarvesterGoRef;
    [HideInInspector] public Harvester HarvesterRef;

    private Dictionary<string, GameObject> _harvesters = new Dictionary<string, GameObject>();

    private void Start()
    {
        foreach(HomemadeStringGameObjectPair pair in Harvesters)
        {
            _harvesters.Add(pair.Key, pair.Value);
        }
    }

    /// <summary>
    /// This method instantiates the harvester prefab, sets its scale, its rotation and calls the InitiateNodesToFollow 
    /// method that initialize harvester path.
    /// </summary>
    public void InstantiateHarvester()
    {
        HarvesterGoRef = Instantiate(_harvesters[GameManager.Instance.MapManager.CurrentMap.name], GameManager.Instance.MapManager.CurrentMap.HarvesterStartPosition.position, Quaternion.identity, transform);

        //Scale fix
        /*        HarvesterGoRef.transform.localScale = GameManager.Instance.MapManager.CurrentMap.gameObject.transform.localScale;*/
        HarvesterGoRef.transform.rotation = GameManager.Instance.MapManager.CurrentMap.HarvesterStartPosition.rotation;
        
        HarvesterRef = HarvesterGoRef.GetComponent<Harvester>();
        HarvesterRef.InitiateNodesToFollow(GameManager.Instance.MapManager.CurrentMap.HarvesterNodes);
    }
}

[Serializable]
public class HomemadeStringGameObjectPair
{
    public string Key;
    public GameObject Value;

    public HomemadeStringGameObjectPair(string key, GameObject value)
    {
        Key = key;
        Value = value;
    }
}
