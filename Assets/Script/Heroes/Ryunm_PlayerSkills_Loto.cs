using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Ryunm_PlayerSkills_Loto : MonoBehaviour {
    /* Skill Introdection
     * Skill 1: Use Energy to recovery
     * Skill 2: Speed Down, No Damage Suffer
     */
    // PlayerControls
    PlayerControlls playerControlls;
    InputAction skill_1;
    InputAction skill_2;
    private void Awake() {
        playerControlls = new PlayerControlls();
        skillOneSlider.value = skillTwoSlider.value = 1;
    }
    private void OnEnable() {
        skill_1 = playerControlls.FPS_Player_Controller.Skill_1;
        skill_1.Enable();
        skill_2 = playerControlls.FPS_Player_Controller.Skill_2;
        skill_2.Enable();  
    }
    private void OnDisable() {
        skill_1.Disable();
        skill_2.Disable();
    }

    // Skill
    [SerializeField] Slider skillOneSlider;
    [SerializeField] Slider skillTwoSlider;

    

    [SerializeField] bool isSkillOne = false;
    [SerializeField] bool isSkillTwo = false;

    [SerializeField] float skillOneActiveTime = 4;
    [SerializeField] float skillTwoActiveTime = 2;
    [SerializeField] float skillOneRecoveryTime = 10;
    [SerializeField] float skillTwoRecoveryTime = 5;

    [SerializeField] float HealingValue = 50;
    [SerializeField] float DamageValue = 30;
    public float moveSpeed = 0.1f;


    [SerializeField] Ryunm_PlayerController playerController;
    [SerializeField] Ryunm_HealthController healthController;

    public void SkillActive() {
        if (skill_1.triggered && skillOneSlider.value > 0) {
            isSkillOne = true;
            StopCoroutine(SkillOne());
            StartCoroutine(SkillOne());
        }
        if (isSkillOne && skillOneSlider.value == 0) {
            isSkillOne = false;
            StopCoroutine(SkillOne());
            StartCoroutine(SkillOneRecovery());
            if (skillOneSlider.value == 1) {
                StopCoroutine(SkillOneRecovery());
            }
        }

        if (skill_2.triggered && skillTwoSlider.value > 0) {
            isSkillTwo = true;
            StopCoroutine(SkillTwo());
            StartCoroutine(SkillTwo());
        }
        if (isSkillTwo && skillTwoSlider.value == 0) {
            isSkillTwo = false;
            StopCoroutine(SkillTwo());
            StartCoroutine(SkillTwoRecovery());
            if (skillTwoSlider.value == 1) {
                StopCoroutine(SkillTwoRecovery());
            }
        }
    }
    IEnumerator SkillOneRecovery() {
        yield return new WaitForSecondsRealtime(skillOneRecoveryTime);
        while (skillOneSlider.value < 1) {
            skillOneSlider.value += 1 / skillOneRecoveryTime * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator SkillTwoRecovery() {
        yield return new WaitForSecondsRealtime (skillTwoRecoveryTime);
        while(skillTwoSlider.value < 1) {
            skillTwoSlider.value += 1 / skillTwoRecoveryTime * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator SkillOne() {
        yield return null;
        healthController.Health(HealingValue);
        playerController.currentEnergy = 0;
        playerController.energySlider.value = playerController.currentEnergy / playerController.maxEnergy;
        skillOneSlider.value = 0;
    }
    IEnumerator SkillTwo() {
        float currentTime = skillTwoActiveTime;
        float minusTime = skillTwoActiveTime / currentTime * Time.deltaTime;
        float originSpeed = playerController.moveSpeed;
        playerController.moveSpeed = moveSpeed;
        while (currentTime > 0) {
            currentTime -= minusTime;
            healthController.Health(1000);

            if (skillTwoSlider) {
                skillTwoSlider.value = currentTime / skillTwoActiveTime;
            }
            yield return null;
        }
        playerController.moveSpeed = originSpeed;
    }
    
}
