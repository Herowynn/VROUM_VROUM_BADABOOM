using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Option is a script that is used for the menu Options.
/// It contains various functions that changes the global sound mixer of the game, the resolution, etc...
/// This script allows you to personalize your in-game settings.
/// </summary>

public class Options : MonoBehaviour
{
    [Header("Instance")]
    public AudioMixer AudioMixer;
    public TMP_Dropdown ResolutionDropdown;
    private Resolution[] _resolutions;
    
    private void Start()
    {
        SetQuality(0);
        CreateResolutionItems();
    }

    private void CreateResolutionItems()
    {
        _resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0, cpt = 0;
        
        foreach (var resolution in _resolutions)
        {
            options.Add(resolution.width + " x " + resolution.height);

            if (resolution.width == Screen.currentResolution.width &&
                resolution.height == Screen.currentResolution.height)
                currentResolutionIndex = cpt;

            cpt++;
        }
        
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume) 
    {
        AudioMixer.SetFloat("MainVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
