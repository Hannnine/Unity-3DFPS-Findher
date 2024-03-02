using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teach_Move : MonoBehaviour {
    PlayerControlls playerControlls;
    InputAction movement;
    InputAction rotation;
    InputAction jump;
    InputAction run;
    InputAction squat;
    InputAction fire;
    InputAction aim;
    InputAction reload;
    InputAction closeAttack;
    InputAction skill_1;
    InputAction skill_2;
    private void Awake() {
        playerControlls = new PlayerControlls();
    }
    private void OnEnable() {
        movement = playerControlls.FPS_Player_Controller.Move;
        movement.Enable();
        rotation = playerControlls.FPS_Player_Controller.Rotation;
        rotation.Enable();
        jump = playerControlls.FPS_Player_Controller.Jump;
        jump.Enable();
        run = playerControlls.FPS_Player_Controller.Run;
        run.Enable();
        squat = playerControlls.FPS_Player_Controller.Squat;
        squat.Enable();
        fire = playerControlls.FPS_Player_Controller.Fire;
        fire.Enable();
        aim = playerControlls.FPS_Player_Controller.Aim;
        aim.Enable();
        reload = playerControlls.FPS_Player_Controller.Reload;
        reload.Enable();
        closeAttack = playerControlls.FPS_Player_Controller.CloseAttack;
        closeAttack.Enable();
        skill_1 = playerControlls.FPS_Player_Controller.Skill_1;
        skill_1.Enable();
        skill_2 = playerControlls.FPS_Player_Controller.Skill_2;
        skill_2.Enable();
    }
    private void OnDisable() {
        movement.Disable();
        rotation.Disable();
        jump.Disable();
        run.Disable();
        squat.Disable();
        fire.Disable();
        aim.Disable();
        reload.Disable();
        closeAttack.Disable();
        skill_1.Disable();
        skill_2.Disable();
    }

    public GameObject JumpHint;
    public GameObject SquatHint;
    public GameObject FireHint;
    public GameObject CloseAttackHint;
    public GameObject SkillHint;
    public GameObject Enemy01;
    public GameObject Enemy02;

    public bool shouldJump;
    public bool shouldRun;
    public bool shouldFire;
    public bool shouldCloseAttack;
    public bool shouldSkill;

    private void Update() {
        if (run.triggered && !shouldJump) {
            shouldJump = true;
            JumpHint.SetActive(false);
            SquatHint.SetActive(true);
        }
        if (squat.triggered && !shouldRun) {
            shouldRun = true;
            SquatHint.SetActive(false);
            FireHint.SetActive(true);
        }
        if (fire.triggered && !shouldFire) { 
            shouldFire = true;
            FireHint.SetActive(false);
            CloseAttackHint.SetActive(true);
        }
        if (closeAttack.triggered && !shouldCloseAttack) {
            shouldCloseAttack = true;
            CloseAttackHint.SetActive (false);
            SkillHint.SetActive(true);
        }
        if (skill_2.triggered && !shouldSkill) {
            shouldSkill = true;
            SkillHint.SetActive(false);
            Enemy01.SetActive(true);
            Enemy02.SetActive(true);
        }
    }
}
