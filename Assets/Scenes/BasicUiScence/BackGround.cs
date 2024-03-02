using System.Collections;
using UnityEngine;

public class BackGround : MonoBehaviour {
    public GameObject BackGroundtext;

    private void Start() {
        StartCoroutine(ShowBackGroundAfterDelay(5));
    }

    private IEnumerator ShowBackGroundAfterDelay(float delay) {
        yield return new WaitForSecondsRealtime(delay);
        BackGroundtext.SetActive(true);
    }
}