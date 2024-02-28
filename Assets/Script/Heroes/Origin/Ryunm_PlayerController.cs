using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ryunm_PlayerController : MonoBehaviour {
    // Mouse Part
    [SerializeField] float rotateSpeed = 180;
    [SerializeField] [Range(1,2)] float rotateRate;

    [SerializeField] Transform Player_Transform;
    [SerializeField] Transform EyeView_Transform;
    float RotateOffset_X;   // axis X shift

    // Keyboard Part
    [SerializeField] CharacterController playerCC;
    [SerializeField] float moveSpeed = 0.25f;
    [SerializeField] float originSpeed = 0.25f;
    [SerializeField] float Gravity = -9.8f;
    [SerializeField] float ver_Velocity = 0;
    [SerializeField] float MAX_HEIGHT = 0.1f;
    

    // Check
    [SerializeField] bool isGround = true;
    private bool shouldJump = false;

    // Animator
    public Ryunm_HoverBotAnimatorController animatorController;
    public Transform enemyCheckPoint;

    // Run Model
    [SerializeField] float runSpeed = 0.7f;
    [SerializeField] float currentEnergy;
    [SerializeField] float maxEnergy = 100;
    [SerializeField] float runRange = 5f;
    [SerializeField] float recoveryInterval = 3;
    [SerializeField] float recoveryTime = 2;
    [SerializeField] bool isRunning;
    [SerializeField] Slider energySlider;

    // Skills
    [SerializeField] Ryunm_PlayerSkills skillsController;

    // Audio
    [SerializeField] AudioSource stepSource;
    [SerializeField] AudioSource jumpSource;
    [SerializeField] AudioSource landSource;


    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        animatorController = GetComponent<Ryunm_HoverBotAnimatorController>();

        currentEnergy = maxEnergy;
        energySlider.value = 1;
        if (energySlider) {
            energySlider.value = currentEnergy / maxEnergy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        JumpCheck();
        PlayerRun();
        skillsController.SkillActive();

    }

    private void FixedUpdate() {

        PlayerRotateControl();
        PlayerMovement();
        ApplyJump();
        PlayerEnergy();
    }

    private void PlayerRotateControl() {
        if (Player_Transform == null || EyeView_Transform == null) return;

        float OffSet_X = Input.GetAxis("Mouse X");  // X controlls Player's horizontal changes (Left n Right)
        float OffSet_Y = Input.GetAxis("Mouse Y");  // Y controlls Eye's vertical changes   (Up n Down)

        // Xshift
        RotateOffset_X += OffSet_Y * rotateSpeed * rotateRate * Time.fixedDeltaTime;
        RotateOffset_X = Mathf.Clamp(RotateOffset_X, -60f, 60f);
        Player_Transform.Rotate(Vector3.up * OffSet_X * rotateSpeed * rotateRate * Time.fixedDeltaTime);

        // Yshift
        Quaternion currentLocalRotation = Quaternion.Euler(new Vector3(RotateOffset_X, EyeView_Transform.localEulerAngles.y, EyeView_Transform.localEulerAngles.z));
        EyeView_Transform.localRotation = currentLocalRotation;
    }
    private void PlayerMovement() {
        if (playerCC == null) return;

        Vector3 motionValue = Vector3.zero;

        // Get keyboard input
        float Input_hor = Input.GetAxis("Horizontal");
        float Input_ver = Input.GetAxis("Vertical");

        // X, Z
        motionValue += transform.forward * moveSpeed * Input_ver; // Front n Behind
        motionValue += transform.right * moveSpeed * Input_hor;   // Left n Right

        /* Y */
        ver_Velocity += Gravity * Time.fixedDeltaTime;
        motionValue += Vector3.up * ver_Velocity;

        // CheckGround
        isGround = playerCC.isGrounded;

        


        playerCC.Move(motionValue);

        // Evalute the Animator
        if (animatorController) {
            animatorController.moveSpeed = moveSpeed * Input_ver;
            animatorController.Alerted = Input_ver == 0 ? false : true;
        }
    }
    private void JumpCheck() {
        //Check Jump
        if (Input.GetButtonDown("Jump")) {
            shouldJump = true;
        }
    }
    private void ApplyJump() {
        float time = 2 * Mathf.Sqrt(2 * MAX_HEIGHT / -Gravity);
        // Make sure Jump smoothly
        if (shouldJump && isGround) {
            ver_Velocity = Mathf.Sqrt(2 * -Gravity * MAX_HEIGHT);
            shouldJump = false; // reset
            if (jumpSource && landSource) {
                jumpSource.Play();
                Invoke("PlayLandSound", time);
            }
        }
    }
    private void PlayerRun() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && energySlider.value > 0) {
            isRunning = true;
            moveSpeed = runSpeed;
            StartCoroutine("UseEnergy");


            if (stepSource) {
                stepSource.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            isRunning = false;
            moveSpeed = originSpeed;
            StopCoroutine("UseEnergy");

            if (stepSource) {
                stepSource.Stop();
            }
        }
    }
    private void PlayerEnergy() {
        if (!isRunning && currentEnergy < maxEnergy) {
            StartCoroutine("RecoveryEnergy");
        }
        else {
            StopCoroutine("RecoveryEnergy");
        }
    }
    IEnumerator UseEnergy() {
        while(isRunning) {
            currentEnergy -= maxEnergy / runRange * Time.deltaTime;
            if (energySlider) {
                energySlider.value = currentEnergy / maxEnergy;
            }
            yield return null;
        }
    }
    IEnumerator RecoveryEnergy() {
        yield return new WaitForSeconds(recoveryInterval);
        while (!isRunning && currentEnergy < maxEnergy) {
            currentEnergy += maxEnergy / recoveryTime * Time.deltaTime;
            if (energySlider) {
                energySlider.value = currentEnergy / maxEnergy;
            }
            yield return null;
        }
    }
    private void PlayStepSource() {
        if (stepSource) {
            stepSource.Play();
        }
    }
    private void PlayLandSound() {
        landSource.Play();
    }
}
