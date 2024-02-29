using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_CloseAttackController : MonoBehaviour {
    public BulletType bulletType = BulletType.Player_Bullet;
    public float P2EdamaValue = 5;
    public float P2EdamaValue_origin = 5;
    [SerializeField] float E2PdamaValue = 1;

    [SerializeField] GameObject attackExplosion;

    [SerializeField] AudioSource attackSource;

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
                    PlayStepSource();
                }
                if (collision.gameObject.CompareTag("Enemy_Turret")) {
                    collision.gameObject.GetComponent<Ryunm_HealthController>().Damage(P2EdamaValue);
                    collision.gameObject.GetComponent<Ryunm_EnemyController_Turretver>().enemyAni.TriggerOnDamage(); //Trigger onAttack
                    PlayStepSource();
                }

                break;
        }

        BornAttackExplosion();
    }
    private void BornAttackExplosion() {
        if (attackExplosion) {
            GameObject newExplosion = Instantiate(attackExplosion, this.transform.position, attackExplosion.transform.rotation);
            Destroy(newExplosion,2);
        }
    }

    private void PlayStepSource() {
        if (attackSource) {
            attackSource.Play();
        }
    }
}
