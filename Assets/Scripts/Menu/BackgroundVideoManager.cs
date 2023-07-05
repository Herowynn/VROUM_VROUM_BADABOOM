using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class BackgroundVideoManager : MonoBehaviour
{
    [Header("Instances")] [SerializeField] private VideoClip[] _videoClips;
    [SerializeField] private VideoPlayer _videoPlayer;
    
    #region Intern Var
    
    private int _currentVideoClipIndex;
    
    #endregion

    private void Start()
    {
        GetRandomVideoClip();
    }

    private void Update()
    {
        if (!_videoPlayer.isPlaying)
            ChangeVideoClip();
    }

    private void GetRandomVideoClip()
    {
        _currentVideoClipIndex = Random.Range(0, _videoClips.Length);
        
        SetCurrentVideoPlayerClip();
        PlayCurrentVideoClip();
    }

    private void ChangeVideoClip()
    {
        _currentVideoClipIndex++;

        if (_currentVideoClipIndex >= _videoClips.Length)
            _currentVideoClipIndex = 0;
        
        SetCurrentVideoPlayerClip();
        PlayCurrentVideoClip();
    }

    private void SetCurrentVideoPlayerClip()
    {
        _videoPlayer.clip = _videoClips[_currentVideoClipIndex];
    }

    private void PlayCurrentVideoClip()
    {
        if (_videoPlayer.clip == null)
            throw new Exception("No clip in video player");
        
        _videoPlayer.Play();
    }
}
