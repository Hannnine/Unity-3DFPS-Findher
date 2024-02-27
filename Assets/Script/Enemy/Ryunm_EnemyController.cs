using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Ryunm_EnemyController : MonoBehaviour {
    // Define
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] GameObject player;

    // Patrol
    [SerializeField] GameObject wayPointParent;
    [SerializeField] Transform[] wayPoints;
    [SerializeField] int nextIndex = 0;

    // Player
    [SerializeField] Ryunm_PlayerController _playerController;
    [SerializeField] float minDistance = 10;

    // Enemy Fire
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletStartPoint;
    [SerializeField] float minAngle = 15;
    [SerializeField] float fireInterval = 2;
    [SerializeField] float bulletStartSpeed = 10;
    public bool isFire;
    public bool isAlert;

    // Animator
    public Ryunm_HoverBotAnimatorController enemyAni;   //This is a script

    // Audio
    [SerializeField] AudioSource bulletSource = null;
    [SerializeField] AudioSource alertSource = null;
    [SerializeField] AudioSource moveSource = null;

    // Start is called before the first frame update
    void Start()
    {
        // Get Player
        _playerController = GameObject.FindObjectOfType<Ryunm_PlayerController>();
        enemyAni = GetComponent<Ryunm_HoverBotAnimatorController>();

        // Get gameObject
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAgent.autoBraking = false;
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Get Point
        if(wayPointParent != null ) {
            // To avoid the Parent-transform;
            wayPoints = wayPointParent.GetComponentsInChildren<Transform>(true)
                .Where(t => t != wayPointParent.transform)
                .ToArray();
            SetNextDestination();
        }

    }

    // Update is called once per frame
    void Update()
    {
        EnemyPatrol();
        EnemyAlert();
        FireController();
    }

    private void SetNextDestination() {
        if (wayPoints.Length <= 1) return;
        enemyAgent.SetDestination(wayPoints[nextIndex].position);
        nextIndex = (nextIndex + 1) % wayPoints.Length;
    }

    private void EnemyPatrol() {
        if (!enemyAgent.pathPending && enemyAgent.remainingDistance < 2f) {
            // Only set next destination if not currently on a path and reached destination
            SetNextDestination();
        }
    }

    private void EnemyAlert() {
        isAlert = Vector3.Distance(transform.position, _playerController.enemyCheckPoint.position) < minDistance ? true : false;
        if (isAlert) {
            // Attack
            enemyAgent.SetDestination(_playerController.enemyCheckPoint.position);
            // Speed
            enemyAni.moveSpeed = enemyAgent.speed;
            enemyAni.Alerted = isAlert;
            if (alertSource) {
                alertSource.Play();
            }
            if (moveSource) {
                moveSource.Play();
            }
        }
        else {
            // Only patrol if not currently alert
            enemyAni.Alerted = isAlert;
            EnemyPatrol();
            if (moveSource) {
                moveSource.Stop();
            }
        }
    }

    private void FireController() {
        // Check whether player is in the range of shoot
        var direction = (_playerController.enemyCheckPoint.position - bulletStartPoint.position).normalized;
        bool Judge = Vector3.Angle(direction, bulletStartPoint.forward) < minAngle ? true : false;
        if (Judge) {
            if (isAlert && !isFire) {
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
        yield return new WaitForSeconds(fireInterval);
        while (isFire) {
            // Update direction to player
            targetDirection = (_playerController.enemyCheckPoint.position - bulletStartPoint.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);

            // Instantiate bullet with updated rotation
            GameObject newBullet = Instantiate(bullet, bulletStartPoint.position, rotation);
            newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * bulletStartSpeed;
            newBullet.GetComponent<Ryunm_BulletController>().bulletType = BulletType.Enemy_Bullet;
            PlayBulletSource();
            Destroy(newBullet, 5);

            // Fire animator
            enemyAni.TriggerAttack();

            yield return new WaitForSeconds(fireInterval);
        }
    }
    private void PlayBulletSource() {
        if (bulletSource) {
            bulletSource.Play();
        }
    }
}
