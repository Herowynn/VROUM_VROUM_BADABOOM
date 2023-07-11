using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapsTextureChanger))]
public class MapsTextureChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapsTextureChanger script = (MapsTextureChanger)target;

        if (GUILayout.Button("Change Textures"))
            script.ChangeTexturesForMapsInList();
    }
}
