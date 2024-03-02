using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toEnd : MonoBehaviour
{
    private void Start() {
        StartCoroutine(ShowBackGroundAfterDelay(180));
    }

    private IEnumerator ShowBackGroundAfterDelay(float delay) {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(20);
    }
}
