using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BulletType {
    Player_Bullet = 0,
    Enemy_Bullet = 1,
}

public class Ryunm_BulletController : MonoBehaviour {
    public BulletType bulletType = BulletType.Player_Bullet;
    public float P2EdamaValue = 10;
    public float E2PdamaValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Physical Crush Check
    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        switch (bulletType) {
            //Player shoot
            case BulletType.Player_Bullet:
                Debug.Log("Player bullet collided with: " + collision.gameObject.name);
                if (collision.gameObject.CompareTag("Enemy")) {
                    collision.gameObject.GetComponent<Ryunm_HealthController>().Damage(P2EdamaValue);
                }
                break;

            //Enemy shoot
            case BulletType.Enemy_Bullet:
                Debug.Log("Enemy bullet collided with: " + collision.gameObject.name);
                if (collision.gameObject.CompareTag("Player")) {
                    collision.gameObject.GetComponent<Ryunm_HealthController>().Damage(E2PdamaValue);
                }
                break;
        }
        Destroy(gameObject);
    }
}
