using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Ryunm_GameManager : MonoBehaviour {
    public GameObject enemy;
    public GameObject wayPointParent;
    public Transform[] wayPoints;
    public float maxPointCount;
    public float currentPointCount;
    public float bornPointCount;
    public float limitCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        // Get Point
        if (wayPointParent != null) {
            // To avoid the Parent-transform;
            wayPoints = wayPointParent.GetComponentsInChildren<Transform>(true)
                .Where(t => t != wayPointParent.transform)
                .ToArray();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BornEnemy() {
        while (currentPointCount < limitCount && bornPointCount < maxPointCount) {
            int index = Random.Range(0, wayPoints.Length);
            GameObject newNemey = Instantiate(enemy);


            yield return null;
        }
    }
}
