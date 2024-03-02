using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ryunm_SettingControllerInGame : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the volume slider in the settings scene
    private bool settingsOpen = false;
    private float initialVolume; // Store initial volume value when opening settings

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!settingsOpen)
            {
                // Open the settings scene
                OpenSettings();
            }
            else
            {
                // Close the settings scene
                CloseSettings();
            }
        }
    }

    void OpenSettings()
    {
        // Load the settings scene
        SceneManager.LoadScene(3, LoadSceneMode.Additive); // Load scene with index 3
        settingsOpen = true;
        Time.timeScale = 0f; // Pause the game

        // Save the initial volume value when opening settings
        initialVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        // Apply the saved volume when opening the settings scene
        volumeSlider.value = initialVolume;
    }

    void CloseSettings()
    {
        // Save the volume slider value before closing the settings scene
        float currentVolume = volumeSlider.value;
        if (currentVolume != initialVolume)
        {
            PlayerPrefs.SetFloat("Volume", currentVolume);
            PlayerPrefs.Save();
        }

        // Unload the settings scene
        SceneManager.UnloadSceneAsync(3); // Unload scene with index 3
        settingsOpen = false;
        Time.timeScale = 1f; // Resume the game
    }
}
