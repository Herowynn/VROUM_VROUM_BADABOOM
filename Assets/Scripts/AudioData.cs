using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName ="Scriptable Objects/Create AudioData")]
public class AudioData : ScriptableObject
{
    public string AudioName;
    public AudioType AudioType;
    public AudioClip[] AudioClips;
}
