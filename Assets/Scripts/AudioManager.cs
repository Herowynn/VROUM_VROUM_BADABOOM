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

    #region Intern Variables

    private GameObject _currentMusic;

    #endregion
    
    private void PlayAudioClipAsMusic(AudioClip audioClip, bool loopMusic)
    {
        if (_currentMusic != null)
            StopCurrentAudioClip();
        
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

            _currentMusic = go;
        }
    }

    private void PlayAudioClipAsSfx(AudioClip audioClip)
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
                    {
                        return sound.AudioClips.Length > 1 ? sound.AudioClips[Random.Range(0, sound.AudioClips.Length)] : sound.AudioClips[0];
                    }
                }
                break;
            case AudioType.MUSIC:
                foreach (var sound in _musicSounds)
                {
                    if (sound.AudioName == audioClipName)
                        return sound.AudioClips.Length > 1 ? sound.AudioClips[Random.Range(0, sound.AudioClips.Length)] : sound.AudioClips[0];
                }
                break;
            default:
                throw new Exception("Audio Type not recognized");
        }

        return null;
    }

    private void StopCurrentAudioClip()
    {
        Destroy(_currentMusic);
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
