using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_HealthPickUpController : MonoBehaviour {
    [SerializeField] float healthCureCount = 50;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Ryunm_HealthController>().Health(healthCureCount);
        }
        Destroy(gameObject);
    }
}
