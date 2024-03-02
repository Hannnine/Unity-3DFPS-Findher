using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ryunm_Settings : MonoBehaviour {
    public Slider bright;
    public Slider DPI;
    public Slider sounds;
    public void BrightSlider(float brightness) {
        PlayerPrefs.SetFloat("Brightness", brightness);
        Debug.Log(brightness);
        bright.value = brightness;
    }
    public void DPISlider(float DPI) {
        PlayerPrefs.SetFloat("DPI", DPI);
        Debug.Log(DPI);
        this.DPI.value = DPI;
    }
    public void SoundsSlider(float sounds) {
        PlayerPrefs.SetFloat("Sounds", sounds);
        Debug.Log(sounds);
        this.sounds.value = sounds;
    }
}
