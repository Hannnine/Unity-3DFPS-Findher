using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ryunm_HealthController : MonoBehaviour {
    [SerializeField] float MAX_HEALTH = 100;
    [SerializeField] float health = 100;

    [SerializeField] Slider healthSlider;

    [SerializeField] GameObject botExplosion;

    [SerializeField] AudioSource deathSource = null;

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
        if (healthSlider) {
            healthSlider.value = health / MAX_HEALTH;
        }
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
            BornBotExplosion();
            if (deathSource) {
                deathSource.Play();
            }

            Destroy(this.gameObject);
            //Death
        }
    }

    public void Health(float cure) {
        if (health > 0) {
            if (health + cure > MAX_HEALTH) {
                health = MAX_HEALTH;
            }
            else {
                health += cure;
            }

            if (healthSlider) {
                healthSlider.value = health / MAX_HEALTH;
            }
        }
    }


    private void BornBotExplosion() {
        if (botExplosion) {
            GameObject newExplosion = Instantiate(botExplosion, this.transform.position, botExplosion.transform.rotation);
            Destroy(newExplosion, 2);
        }
    }
}
