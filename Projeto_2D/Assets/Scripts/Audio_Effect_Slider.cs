using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Audio_Effect_Slider : MonoBehaviour
{
    public Slider volumeSlider;
    public List<AudioSource> audioSources;

    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
            UpdateVolume(volumeSlider.value);
        }
    }

    void UpdateVolume(float value)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null)
            {
                audioSource.volume = value;
            }
        }
    }

    void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(UpdateVolume);
        }
    }
}
