using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ryunm_HealthController : MonoBehaviour {
    [SerializeField] float MAX_HEALTH = 100;
    [SerializeField] float health = 100;

    [SerializeField] Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
        if (healthSlider) {
            healthSlider.value = health / MAX_HEALTH;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage) {
        
        if(health > 0) {
            health -= damage;
            if (healthSlider) {
                healthSlider.value = health / MAX_HEALTH;
            }

        }
        if(health <= 0) { 
            health = 0;
            //Death
        }
    }
}
