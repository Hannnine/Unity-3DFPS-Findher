using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ryunm_HealthController : MonoBehaviour {
    [SerializeField] GameObject HealthHolder;
    public float MAX_HEALTH = 100;
    public float health;

    [SerializeField] Slider healthSlider;

    [SerializeField] GameObject botExplosion;
    [SerializeField] AudioSource deathSource = null;

    //Pickup
    [SerializeField] GameObject healthPackPrefab;
    [SerializeField] float healthPackRadius = 1f; // set range
    public bool shouldHealthPack = false;


    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
        if (healthSlider) {
            healthSlider.value = health / MAX_HEALTH;
        }

        shouldHealthPack = HealthHolder.gameObject.CompareTag("Enemy_Turret") ? true : false;
        shouldHealthPack = HealthHolder.gameObject.CompareTag("Enemy_HovreBot") ? true : false;
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
            if (this.gameObject.CompareTag("Player")) {
                SceneManager.LoadScene(13);
            }
            Destroy(this.gameObject);
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
    public void HealthPack(bool shouldHealthPack) {
        // Instantiate a health pack
        if (shouldHealthPack && healthPackPrefab) {
            Vector3 healthPackPosition = transform.position + Vector3.up * 0.5f;
            Instantiate(healthPackPrefab, healthPackPosition, Quaternion.identity);
        }
    }
}
