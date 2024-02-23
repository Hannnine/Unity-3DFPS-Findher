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

    // Start is called before the first frame update
    void Start()
    {
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
