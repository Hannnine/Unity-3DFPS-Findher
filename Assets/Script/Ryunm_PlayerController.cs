using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_PlayerController : MonoBehaviour {
    // Mouse Part
    [SerializeField] float rotateSpeed = 180;
    [SerializeField] [Range(1,2)] float rotateRate;

    [SerializeField] Transform Player_Transform;
    [SerializeField] Transform EyeView_Transform;
    float RotateOffset_X;   // axis X shift

    // Keyboard Part
    [SerializeField] CharacterController playerCC;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float Gravity = -9.8f;
    [SerializeField] float ver_Velocity = 0;
    [SerializeField] float MAX_HEIGHT = 0.1f;
    

    // Check
    [SerializeField] bool isGround = true;
    private bool shouldJump = false;

    // Animator
    public Ryunm_HoverBotAnimatorController animatorController;
    public Transform enemyCheckPoint;


    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        animatorController = GetComponent<Ryunm_HoverBotAnimatorController>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpCheck();
        
    }

    private void FixedUpdate() {
        PlayerRotateControl();
        PlayerMovement();
        ApplyJump();
        
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
        // Make sure Jump smoothly
        if (shouldJump && isGround) {
            ver_Velocity = Mathf.Sqrt(2 * -Gravity * MAX_HEIGHT);
            shouldJump = false; // reset
        }
    }

    public void OnDamage() {
        animatorController.TriggerOnDamage();
    }

}
