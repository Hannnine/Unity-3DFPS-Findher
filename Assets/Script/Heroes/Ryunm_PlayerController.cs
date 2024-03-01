    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Ryunm_PlayerController : MonoBehaviour {
    // PlayerControls
    PlayerControlls playerControlls;
    InputAction movement;
    InputAction rotation;
    InputAction jump;
    InputAction run;
    InputAction squat;
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
    }
    private void OnDisable() {
        movement.Disable();
        rotation.Disable();
        jump.Disable();
        run.Disable();
        squat.Disable();
    }

    // Mouse Part
    [SerializeField] float rotateSpeed = 180;
    [SerializeField] [Range(1,2)] float rotateRate;

    [SerializeField] Transform Player_Transform;
    [SerializeField] Transform EyeView_Transform;
    float RotateOffset_X;   // axis X shift

    // Keyboard Part
    [SerializeField] CharacterController playerCC;
    public float moveSpeed = 0.25f;
    public float originSpeed = 0.25f;
    [SerializeField] float Gravity = -9.8f;
    [SerializeField] float ver_Velocity = 0;
    [SerializeField] float MAX_HEIGHT = 0.1f;

    // Animator
    public Ryunm_HoverBotAnimatorController animatorController;
    public Transform enemyCheckPoint;

    // Run Model
    [SerializeField] float runSpeed = 0.7f;
    public float currentEnergy;
    public float maxEnergy = 100;
    [SerializeField] float runRange = 5f;
    [SerializeField] float recoveryInterval = 3;
    [SerializeField] float recoveryTime = 2;
    [SerializeField] bool isRunning;
    public Slider energySlider;

    // Squat Model
    [SerializeField] bool isSquat = false;
    [SerializeField] Vector3 defaultEyePoint;
    [SerializeField] Vector3 squatEyePoint;
    [SerializeField] float squatRatio = 0.5f;
    [SerializeField] Transform eyeView;

    // Skills
    [SerializeField] bool isBallistic;
    [SerializeField] bool isOctane;
    [SerializeField] bool isLoto;

    [SerializeField] Ryunm_PlayerSkills skillsController;
    [SerializeField] Ryunm_PlayerSkills_Octane skillsController_Octane;
    [SerializeField] Ryunm_PlayerSkills_Loto skillsController_Loto;
    [SerializeField] Ryunm_HealthController healthController;

    // Audio
    [SerializeField] AudioSource stepSource;
    [SerializeField] AudioSource jumpSource;
    [SerializeField] AudioSource landSource;

    
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        animatorController = GetComponent<Ryunm_HoverBotAnimatorController>();

        currentEnergy = maxEnergy;
        energySlider.value = 1;
        if (energySlider) {
            energySlider.value = currentEnergy / maxEnergy;
        }
        string playerHeroChoice = PlayerPrefs.GetString("PlayerChoice");
        //Define Health
        if (playerHeroChoice == "Ballistic") { healthController.MAX_HEALTH = 150; healthController.health = 150; isBallistic = true; isOctane = isLoto = false;}
        else if (playerHeroChoice == "Octane") { healthController.MAX_HEALTH = 100; healthController.health = 100; isOctane = true; isBallistic = isLoto = false; }
        else if (playerHeroChoice == "Loto") { healthController.MAX_HEALTH = 200; healthController.health = 200; isLoto = true; isBallistic = isOctane = false; }
    }

    // Update is called once per frame
    void Update()
    {
        HandleJump();
        PlayerRun();
        PlayerSquat();
        HeroSkillSelect();
    }

    private void FixedUpdate() {

        PlayerRotateControl();
        PlayerMovement();
        PlayerEnergy();
    }

    private void PlayerRotateControl() {
        if (Player_Transform == null || EyeView_Transform == null) return;

        // Get Input
        Vector2 rotationInput = rotation.ReadValue<Vector2>();
        float OffSet_X = rotationInput.x;
        float OffSet_Y = rotationInput.y;

        float mouseX = Input.GetAxisRaw("Mouse X") * rotateSpeed * rotateRate * Time.fixedDeltaTime;
        float mouseY = -Input.GetAxisRaw("Mouse Y") * rotateSpeed * rotateRate * Time.fixedDeltaTime;

        OffSet_X += mouseX;
        OffSet_Y += mouseY;

        // Xshift
        RotateOffset_X += OffSet_Y * rotateSpeed * rotateRate * Time.fixedDeltaTime;
        RotateOffset_X = Mathf.Clamp(RotateOffset_X, -60f, 60f);
        Player_Transform.Rotate(Vector3.up * OffSet_X * rotateSpeed * rotateRate * Time.fixedDeltaTime);

        // Yshift
        Quaternion currentLocalRotation = Quaternion.Euler(new Vector3(-RotateOffset_X, EyeView_Transform.localEulerAngles.y, EyeView_Transform.localEulerAngles.z));
        EyeView_Transform.localRotation = currentLocalRotation;
    }
    private void PlayerMovement() {
        if (playerCC == null) return;

        Vector2 movementInput = movement.ReadValue<Vector2>();
        float inputHorizontal = movementInput.x;
        float inputVertical = movementInput.y;

        Vector3 motionValue = Vector3.zero;

        // X, Z
        motionValue += transform.forward * moveSpeed * inputVertical; // Forward n behind
        motionValue += transform.right * moveSpeed * inputHorizontal;   // Left n Right

        /* Y */
        ver_Velocity += Gravity * Time.fixedDeltaTime;
        motionValue += Vector3.up * ver_Velocity;


        playerCC.Move(motionValue);

        // Animator
        if (animatorController) {
            animatorController.moveSpeed = moveSpeed * inputVertical;
            animatorController.Alerted = inputVertical == 0 ? false : true;
        }
    }
    private void HandleJump() {
        // JumpCheck
        if (jump.triggered && playerCC.isGrounded) {
            float time = 2 * Mathf.Sqrt(2 * MAX_HEIGHT / -Gravity);
            ver_Velocity = Mathf.Sqrt(2 * -Gravity * MAX_HEIGHT);
            if (jumpSource && landSource) {
                jumpSource.Play();
                Invoke("PlayLandSound", time);
            }
        }
    }
    private void PlayerRun() {
        if (run.triggered && energySlider.value > 0 && !isRunning) {
            // When enough Energy
            isRunning = true;
            moveSpeed = runSpeed;
            StartCoroutine(UseEnergy());

            if (stepSource) {
                stepSource.Play();
            }
        }
        else if ((run.triggered && isRunning) || energySlider.value == 0) {
            // When inenough Energy
            isRunning = false;
            moveSpeed = originSpeed;
            StopCoroutine(UseEnergy());

            if (stepSource) {
                stepSource.Stop();
            }
        }
    }
    private void PlayerSquat() {
        if (squat.triggered) {
            if (!isSquat) {
                isSquat = true;
                StopCoroutine("ViewToDefault");
                StartCoroutine("ViewToSquat");
            }
            else if (isSquat) {
                isSquat = false;
                StopCoroutine("ViewToSquat");
                StartCoroutine("ViewToDefault");
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
    
    private void HeroSkillSelect() {
        if (isBallistic) {
            skillsController.SkillActive();
        }
        else if (isOctane) {
            skillsController_Octane.SkillActive();
        }
        else if (isLoto) {
            skillsController_Loto.SkillActive();
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
    IEnumerator ViewToSquat() {
        while(eyeView.localPosition != squatEyePoint) {
            eyeView.localPosition = Vector3.Lerp(eyeView.localPosition, squatEyePoint, squatRatio);
            yield return null;
        }
    }
    IEnumerator ViewToDefault() {
        while(eyeView.localPosition != defaultEyePoint) {
            eyeView.localPosition = Vector3.Lerp(eyeView.localPosition, defaultEyePoint, squatRatio);
            yield return null;
        }
    }

}
