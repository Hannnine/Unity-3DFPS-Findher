using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_MapDethController : MonoBehaviour
{
    [SerializeField] Ryunm_HealthController healthController;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            healthController.Damage(1000);
        }
    }
}
