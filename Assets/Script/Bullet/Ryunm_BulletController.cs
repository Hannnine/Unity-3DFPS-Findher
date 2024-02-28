using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BulletType {
    Player_Bullet = 0,
    Enemy_Bullet = 1,
}

public class Ryunm_BulletController : MonoBehaviour {
    public BulletType bulletType = BulletType.Player_Bullet;
    [SerializeField] float P2EdamaValue = 10;
    [SerializeField] float E2PdamaValue = 1;

    [SerializeField] GameObject bulletExplosion;

    public bool shouldExplosion = false;
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
        switch (bulletType) {
            //Enemy shoot
            case BulletType.Enemy_Bullet:
                if (collision.gameObject.CompareTag("Player")) {
                    collision.gameObject.GetComponent<Ryunm_HealthController>().Damage(E2PdamaValue);
                    collision.gameObject.GetComponent<Ryunm_PlayerController>().animatorController.TriggerOnDamage();  //Trigger onAttack
                }
                break;
            //Player shoot
            case BulletType.Player_Bullet:
                if (collision.gameObject.CompareTag("Enemy_HovreBot")) {
                    collision.gameObject.GetComponent<Ryunm_HealthController>().Damage(P2EdamaValue);
                    collision.gameObject.GetComponent<Ryunm_EnemyController>().enemyAni.TriggerOnDamage(); //Trigger onAttack
                }
                if (collision.gameObject.CompareTag("Enemy_Turret")) {
                    collision.gameObject.GetComponent<Ryunm_HealthController>().Damage(P2EdamaValue);
                    collision.gameObject.GetComponent<Ryunm_EnemyController_Turretver>().enemyAni.TriggerOnDamage(); //Trigger onAttack
                }

                break;
        }
        BornBulletExplosion();
        
    }
    private void BornBulletExplosion() {
        if (bulletExplosion && shouldExplosion) {
            GameObject newExplosion = Instantiate(bulletExplosion, this.transform.position, bulletExplosion.transform.rotation);
            Destroy(newExplosion,2);
        }
    }
}
