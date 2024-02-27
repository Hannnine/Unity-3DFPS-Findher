using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Ryunm_EnemyController_Turretver : MonoBehaviour {
    // Define
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] GameObject player;

    // Player
    [SerializeField] Ryunm_PlayerController _playerController;
    [SerializeField] float minDistance = 10;

    // Enemy Fire
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletStartPoint;
    [SerializeField] float fireInterval = 2;
    [SerializeField] float bulletStartSpeed = 10;
    public bool isFire;
    public bool isActive;

    // Animator
    public Ryunm_TurretAnimatorController enemyAni;   //This is a script

    // Audio
    [SerializeField] AudioSource bulletSource = null;
    [SerializeField] AudioSource alertSource = null;

    // Start is called before the first frame update
    void Start() {
        // Get Player
        enemyAgent = GetComponent<NavMeshAgent>();
        _playerController = GameObject.FindObjectOfType<Ryunm_PlayerController>();
        enemyAni = GetComponent<Ryunm_TurretAnimatorController>();

        // Get gameObject
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        EnemyActive();
        FireController();
    }

    private void EnemyActive() {
        isActive = Vector3.Distance(transform.position, _playerController.enemyCheckPoint.position) < minDistance ? true : false;
        if (isActive) {
            if (alertSource) {
                alertSource.Play();
            }
            enemyAni.isActive = isActive;
            // Get the position of Player
            Vector3 playerDirection = _playerController.enemyCheckPoint.position - transform.position;
            playerDirection.y = 0;
            playerDirection.Normalize();
            // Agent forward to Player
            enemyAgent.transform.rotation = Quaternion.LookRotation(playerDirection);
        }
        else {
            // Only patrol if not currently alert
            enemyAni.isActive = isActive;
        }
    }

    private void FireController() {
        // Check whether player is in the range of shoot
        var direction = (_playerController.enemyCheckPoint.position - bulletStartPoint.position).normalized;
        bool Judge = Vector3.Distance(transform.position, _playerController.enemyCheckPoint.position) < minDistance ? true : false;
        if (Judge) {
            if (isActive && !isFire) {
                isFire = true;
                StartCoroutine("Fire", direction);
            }
        }
        else {
            if (isFire) {
                isFire = false;
                StopCoroutine("Fire");
            }
        }
    }

    IEnumerator Fire(Vector3 targetDirection) {
        while (isFire) {
            yield return new WaitForSeconds(fireInterval);
            // Update direction to player
            targetDirection = (_playerController.enemyCheckPoint.position - bulletStartPoint.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);

            // Instantiate bullet with updated rotation
            GameObject newBullet = Instantiate(bullet, bulletStartPoint.position, rotation);
            newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * bulletStartSpeed;
            newBullet.GetComponent<Ryunm_BulletController>().bulletType = BulletType.Enemy_Bullet;
            PlayBulletSource();
            Destroy(newBullet, 5);

            yield return new WaitForSeconds(fireInterval);
        }
    }
    private void PlayBulletSource() {
        if (bulletSource) {
            bulletSource.Play();
        }
    }
}
