using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_EnemyController : MonoBehaviour {
    public Ryunm_PlayerController _playerController;


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindObjectOfType<Ryunm_PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
