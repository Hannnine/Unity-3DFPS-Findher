using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_TurretAnimatorController : MonoBehaviour {
    [SerializeField] public bool isActive;      // On a certain range of Player

    public Animator _turretAni;

    // Start is called before the first frame update
    void Start() {
        _turretAni = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        SetParameters();
    }

    private void SetParameters() {
        if (_turretAni == null) return;
        _turretAni.SetBool("IsActive", isActive);
    }
    // When OnFired
    public void TriggerOnDamage() {
        if (_turretAni == null) return;
        _turretAni.SetTrigger("OnDamaged");
    }
}