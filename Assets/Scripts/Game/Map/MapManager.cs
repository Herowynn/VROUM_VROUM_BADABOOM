using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Instance")] 
    public Map[] Maps;
    public Map CurrentMap;

    public Dictionary<string, Map> MapsByName = new Dictionary<string, Map>();

    private void Start()
    {
        foreach (var mapGo in Maps)
        {
            MapsByName.Add(mapGo.Name, mapGo);
        }
    }

    public void LoadMap(string name)
    {
        CurrentMap = MapsByName[name];
        CurrentMap.Load();
    }
}
