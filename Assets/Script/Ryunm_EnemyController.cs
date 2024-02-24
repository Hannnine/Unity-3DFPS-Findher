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
    [SerializeField] bool isFire;
    [SerializeField] bool isAlert;


    // Start is called before the first frame update
    void Start()
    {
        // Get Player
        _playerController = GameObject.FindObjectOfType<Ryunm_PlayerController>();

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
        if (!enemyAgent.pathPending && enemyAgent.remainingDistance < 0.5f) {
            // Only set next destination if not currently on a path and reached destination
            SetNextDestination();
        }
    }

    private void EnemyAlert() {
        if (Vector3.Distance(transform.position, _playerController.enemyCheckPoint.position) < minDistance || isAlert) {
            // Attack
            enemyAgent.SetDestination(_playerController.enemyCheckPoint.position);
            isAlert = true;
        }
        else {
            isAlert = false;
            // Only patrol if not currently alert
            EnemyPatrol();
        }
    }

    private void FireController() {
        // Check whether player is in the range of shoot
        var direction = (_playerController.enemyCheckPoint.position - bulletStartPoint.position).normalized;
        if (Vector3.Angle(direction, bulletStartPoint.forward) < minAngle) {
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
            Destroy(newBullet, 5);

            yield return new WaitForSeconds(fireInterval);
        }
    }
}
