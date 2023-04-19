using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains all the maps, stores a reference of the current map and manages its loading.
/// </summary>
public class MapManager : MonoBehaviour
{
    [Header("Instance")] 
    public Map[] Maps;
    [HideInInspector] public Map CurrentMap;

    private Dictionary<string, Map> _mapsByName = new Dictionary<string, Map>();

    private void Start()
    {
        foreach (var mapGo in Maps)
        {
            _mapsByName.Add(mapGo.Name, mapGo);
        }
    }

    public void LoadMap(string name)
    {
        CurrentMap = _mapsByName[name];
        CurrentMap.Load();
    }
}
