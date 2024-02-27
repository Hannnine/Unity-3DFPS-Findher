using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearTimer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timeText;

    float clearTime;

    void FixedUpdate() {
        clearTime += Time.fixedDeltaTime;

        int minutes = Mathf.FloorToInt(clearTime / 60);
        int seconds = Mathf.FloorToInt(clearTime % 60);
        int milliseconds = Mathf.FloorToInt((clearTime * 1000) % 100);
        string timeString = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

        timeText.text = timeString;
    }
}
