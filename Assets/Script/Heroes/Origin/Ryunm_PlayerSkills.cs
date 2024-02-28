using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ryunm_PlayerSkills : MonoBehaviour {
    /* Skill Introdection
     * Skill 1: Don't need Reload
     * Skill 2: Shoot speed up & bullet explosion
     */

    // Skill
    [SerializeField] Slider skillOneSlider;
    [SerializeField] Slider skillTwoSlider;

    [SerializeField] bool isSkillOne = false;
    [SerializeField] bool isSkillTwo = false;

    [SerializeField] float skillOneActiveTime = 4;
    [SerializeField] float skillTwoActiveTime = 2;
    [SerializeField] float skillOneRecoveryTime = 10;
    [SerializeField] float skillTwoRecoveryTime = 5;
    

    [SerializeField] Ryunm_WeaponController weaponController;
    [SerializeField] Ryunm_BulletController bulletController;


    // Start is called before the first frame update
    void Start()
    {
        skillOneSlider.value = skillTwoSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {

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
        float currentTime = skillOneActiveTime;
        float minusTime = skillOneActiveTime / currentTime * Time.deltaTime;
        while (currentTime > 0) {
            currentTime -= minusTime;
            weaponController.currentAmmoCount = weaponController.maxAmmoCount;
            if (skillOneSlider) {
                skillOneSlider.value = currentTime / skillTwoActiveTime;
            }
            yield return null;
        }
    }
    IEnumerator SkillTwo() {
        float currentTime = skillTwoActiveTime;
        float minusTime = skillTwoActiveTime / currentTime * Time.deltaTime;
        float originFireInterval = weaponController.fireInterval;
        float skillFireInterval = 0.05f;
        while (currentTime > 0) {
            currentTime -= minusTime;
            bulletController.shouldExplosion = true;
            weaponController.fireInterval = skillFireInterval;
            if (skillTwoSlider) {
                skillTwoSlider.value = currentTime / skillTwoActiveTime;
            }
            yield return null;
        }
        bulletController.shouldExplosion = false;
        weaponController.fireInterval = originFireInterval;
    }
    public void SkillActive() {
        if (Input.GetKeyDown(KeyCode.E) && skillOneSlider.value > 0) {
            isSkillOne = true;
            StartCoroutine(SkillOne());
        }
        if(isSkillOne && skillOneSlider.value == 0) {
            StopCoroutine(SkillOne());
            StartCoroutine(SkillOneRecovery());
            if (skillOneSlider.value == 1) {
                StopCoroutine(SkillOneRecovery());
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q) && skillTwoSlider.value > 0) {
            isSkillTwo = true;
            StartCoroutine(SkillTwo());
        }
        if(isSkillTwo && skillTwoSlider.value == 0) { 
            StopCoroutine(SkillTwo());
            StartCoroutine(SkillTwoRecovery());
            if(skillTwoSlider.value == 1) {
                StopCoroutine (SkillTwoRecovery());
            }
        }
    }
}
