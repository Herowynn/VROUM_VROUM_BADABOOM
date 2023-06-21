using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);            
        }
    }

    [Header("Instances")] [SerializeField] private Transform _musicContainer;
    [SerializeField] private Transform _sfxContainer;
    [SerializeField] private AudioMixerGroup _masterMixer;
    [SerializeField] private AudioMixerGroup _musicMixer;
    [SerializeField] private AudioMixerGroup _sfxMixer;

    [Header("Sounds")] [SerializeField] private AudioData[] _sfxSounds;
    [SerializeField] private AudioData[] _musicSounds;
    
    public void PlayAudioClipAsMusic(AudioClip audioClip, bool loopMusic)
    {
        if (audioClip != null)
        {
            GameObject go = new GameObject();
            go.transform.parent = _musicContainer;
            go.name = audioClip.name;

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = audioClip;
            source.outputAudioMixerGroup = _musicMixer;
            source.loop = loopMusic;
            source.Play();
        }
    }

    public void PlayAudioClipAsSfx(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            GameObject go = new GameObject();
            go.transform.parent = _sfxContainer;
            go.name = audioClip.name;

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = audioClip;
            source.outputAudioMixerGroup = _sfxMixer;
            source.Play();
            
            Destroy(go, source.clip.length);
        }
    }

    private AudioClip GetAudioClipByName(string audioClipName, AudioType audioTypeToLookFor)
    {
        switch (audioTypeToLookFor)
        {
            case AudioType.SFX:
                foreach (var sound in _sfxSounds)
                {
                    if (sound.AudioName == audioClipName)
                        return sound.AudioClips[Random.Range(0, sound.AudioClips.Length)];
                }
                break;
            case AudioType.MUSIC:
                foreach (var sound in _musicSounds)
                {
                    if (sound.AudioName == audioClipName)
                        return sound.AudioClips[0];
                }
                break;
            default:
                throw new Exception("Audio Type not recognized");
        }

        return null;
    }

    #region Called Externally
    
    public void PlaySfx(string sfxName)
    {
        PlayAudioClipAsSfx(GetAudioClipByName(sfxName, AudioType.SFX));
    }

    public void PlayMusic(string musicName)
    {
        PlayAudioClipAsMusic(GetAudioClipByName(musicName, AudioType.MUSIC), false);
    }

    public void PlayMusicAndLoop(string musicName)
    {
        PlayAudioClipAsMusic(GetAudioClipByName(musicName, AudioType.MUSIC), true);
    }
    
    #endregion
}
