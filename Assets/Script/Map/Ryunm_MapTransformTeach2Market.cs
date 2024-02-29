using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_MapTransformTeach2Market : MonoBehaviour {
    [SerializeField] Transform inMarketCheckPoint;
    [SerializeField] Transform Player;
    private bool hasTeleported = false;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !hasTeleported) {
            Player.position = inMarketCheckPoint.position;
            hasTeleported = true;
        }
    }
}
