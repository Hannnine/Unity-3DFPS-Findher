using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_GlobalSounds : MonoBehaviour {

    public static float globalVolume = 1.0f;
    float volumSlider;

    void Start() {
        volumSlider = PlayerPrefs.GetFloat("Sounds");
        ApplyGlobalVolume();
    }

    private void Update() {
        volumSlider = PlayerPrefs.GetFloat("Sounds");
        ApplyGlobalVolume();
    }

    public static void ApplyGlobalVolume() {
        AudioListener.volume = globalVolume;
    }

    public void OnVolumeChanged() {
        globalVolume = volumSlider;
        ApplyGlobalVolume();
    }
}
