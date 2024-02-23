using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ryunm_EnemyController : MonoBehaviour {
    // Define
    public NavMeshAgent enemyAgent;
    public GameObject player;

    // Patrol
    public GameObject wayPointParent;
    public Transform[] wayPoints;
    public int nextIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAgent.autoBraking = false;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAgent.destination = wayPoints[nextIndex].position;


        if(wayPointParent != null ) {
            wayPoints = wayPointParent.GetComponentsInChildren<Transform>(false);
            SetNextPoint();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!enemyAgent.pathPending && enemyAgent.remainingDistance < 0.5f) {
            SetNextPoint();
        }
        
    }

    private void SetNextPoint() {
        if (wayPoints.Length <= 1) return;
        enemyAgent.destination = wayPoints[nextIndex].position;
        nextIndex = (nextIndex + 1) % wayPoints.Length;
    }
}
