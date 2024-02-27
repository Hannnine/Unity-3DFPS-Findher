using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_HoverBotAnimatorController : MonoBehaviour {
    [SerializeField] public float moveSpeed;   // Speed of move
    [SerializeField] public bool Alerted;      // On a certain range of Player
    [SerializeField] public bool Death;

    public Animator _hovertAni;

    // Start is called before the first frame update
    void Start() {
        _hovertAni = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        SetParameters();
    }

    private void SetParameters() { 
        if(_hovertAni ==  null) return;
        _hovertAni.SetFloat("MoveSpeed", moveSpeed);
        _hovertAni.SetBool("Alerted", Alerted);
        _hovertAni.SetBool("Death", Death);
    }
    // When OnFired
    public void TriggerAttack() {
        if(_hovertAni == null) return;
        _hovertAni.SetTrigger("Attack");
    }

    public void TriggerOnDamage() {
        if(_hovertAni == null) return;
        _hovertAni.SetTrigger("OnDamaged");
    }
}
