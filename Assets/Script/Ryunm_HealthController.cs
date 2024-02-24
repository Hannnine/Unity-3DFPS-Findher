using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_HealthController : MonoBehaviour {
    public float MAX_HEALTH = 100;
    public float health = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage) {
        
        if(health > 0) {
            health -= damage;
        }
        if(health <= 0) { 
            health = 0;
            //Death
        }
    }
}
