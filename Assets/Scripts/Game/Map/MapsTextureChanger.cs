using InternalRealtimeCSG;
using RealtimeCSG.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapsTextureChanger : MonoBehaviour
{
    #region Public Fields

    public List<MapAndPrefab> MapsToModify = new List<MapAndPrefab>();

    [Header("Default textures")]
    public Material DefaultBorderTexture;
    public Material DefaultRoadTexture;

    [Header("New textures")]
    public Material NewBorderTexture;
    public Material NewRoadTexture;

    #endregion

    #region Textures Modification Methods

    public void ChangeTextures(Map map, GameObject prefabMap)
    {
        List<GameObject> roadParts = new List<GameObject>();
        int roadPartIndex = -1;
        int modelObjectIndex = -1;
        int generatedMeshesIndex = -1;

        for(int i = 0; i < prefabMap.transform.childCount; i++)
        {
            GameObject mapChildObject = prefabMap.transform.GetChild(i).gameObject;
            if (mapChildObject.tag == "RoadPart")
                roadParts.Add(mapChildObject);
        }

        foreach(GameObject roadPart in roadParts)
        {
            roadPartIndex = roadPart.transform.GetSiblingIndex();
            List<GameObject> models = new List<GameObject>();

            for(int i = 0; i < roadPart.transform.childCount; i++)
            {
                GameObject roadPartChild = roadPart.transform.GetChild(i).gameObject;
                if (roadPartChild.GetComponent<CSGModel>())
                    models.Add(roadPartChild);
            }

            foreach(GameObject model in models)
            {
                modelObjectIndex = model.transform.GetSiblingIndex();
                GameObject generatedMeshes = null;

                for(int i = 0; i < model.transform.childCount; i++)
                {
                    GameObject modelChildObject = model.transform.GetChild(i).gameObject;
                    if (modelChildObject.name == "[generated-meshes]")
                    {
                        generatedMeshes = modelChildObject;
                        break;
                    }
                }

                generatedMeshesIndex = generatedMeshes.transform.GetSiblingIndex();

                for(int i = 0; i < generatedMeshes.transform.childCount; i++)
                {
                    GameObject generatedMesh = generatedMeshes.transform.GetChild(i).gameObject;
                    if (generatedMesh.GetComponent<MeshRenderer>())
                    {
                        GeneratedMeshInstance meshInstanceOfPrefab = generatedMesh.GetComponent<GeneratedMeshInstance>();
                        GeneratedMeshInstance meshInstance = map.gameObject.transform.GetChild(roadPartIndex).
                            GetChild(modelObjectIndex).GetChild(generatedMeshesIndex).GetChild(i).gameObject.GetComponent<GeneratedMeshInstance>();

                        if (meshInstanceOfPrefab.RenderMaterial == DefaultBorderTexture)
                            meshInstance.RenderMaterial = NewBorderTexture;

                        if (meshInstanceOfPrefab.RenderMaterial == DefaultRoadTexture)
                            meshInstance.RenderMaterial = NewRoadTexture;
                    }
                }
            }
        }
    }

    public void ChangeTexturesForMapsInList()
    {
        foreach (MapAndPrefab mp in MapsToModify)
            ChangeTextures(mp.Map, mp.Prefab);
    }

    #endregion
}

[Serializable]
public class MapAndPrefab
{
    public GameObject Prefab;
    public Map Map;
}
