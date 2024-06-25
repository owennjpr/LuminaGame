using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    
    private bool canJump;
    public Transform orientation;

    private ConstantForce cForce;
    public float gravityMultiplier;
    private Vector3 gravDirection;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    private Rigidbody rb;
    

    [Header("Light Powers")]
    public GameController gameController;
    private bool readyToSlowfall;
    private bool slowfallInUse;
    public float slowfallMultiplier;
    public float slowfallDelay;

    public float highJumpPower;
    private bool readyToHighJump;
    private bool highJumpInUse; 


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    

    [Header("Audio")]
    public AudioSource jump_audio_player;
    private AudioSource audio_s;
    public AudioClip jump_sfx;
    public AudioClip land_sfx;
    public AudioClip step1_sfx;    
    public AudioClip step2_sfx;
    public float steptimer;
    public int stepIndex;
    
    public MovementState state;
    public enum MovementState {
        walking,
        sprinting,
        air
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cForce = GetComponent<ConstantForce>();
        gravDirection = new Vector3(0, -9.81f, 0);
        cForce.force = gravDirection * gravityMultiplier;
        rb.freezeRotation = true;
        canJump = true;
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        audio_s = GetComponent<AudioSource>();
        steptimer = 0;
        stepIndex = 0;
    }

    private void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(jumpKey) && grounded && canJump) {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (canJump && grounded) {
            highJumpInUse = false;
        }
    }

    private void StateHandler() {
        if(grounded && Input.GetKey(KeyCode.LeftShift)) {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        } else if (grounded) {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        } else {
            state = MovementState.air;
        }
    }
    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //slope check
        if (OnSlope()) {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10, ForceMode.Force);
            if (rb.velocity.y > 0) {
                rb.AddForce(Vector3.down * 80, ForceMode.Force);
            }
        }

        if (grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        } else {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        if (slowfallInUse && grounded) {
            slowfallInUse = false;
            readyToSlowfall = false;
            StartCoroutine(gameController.fadeoutSlowfall());
        }
        if (grounded && state == MovementState.air) {
            jump_audio_player.clip = land_sfx;
            jump_audio_player.Play();
        }
        MyInput();
        StateHandler();
        SpeedControl();
        if (grounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0;
        }
        footstep();
       
    }

    private void FixedUpdate() {
        MovePlayer();
        if (!slowfallInUse) {
            gravityMultiplier = 3;
        }

        if (OnSlope()) {
            // Debug.Log("hiiiiiii");
            cForce.force = gravDirection * 0f;
        } else {
            cForce.force = gravDirection * gravityMultiplier;
        }
        
    }

    private void footstep() {
        steptimer += Time.deltaTime;
        if (rb.velocity.magnitude > 2 && state == MovementState.walking && steptimer > 0.5f) {
            playStep();
            steptimer = 0;
        } else if (rb.velocity.magnitude > 2 && state == MovementState.sprinting && steptimer > 0.3f) {
            playStep();
            steptimer = 0;
        }
    }

    private void playStep() {
        if (stepIndex == 0) {
            audio_s.clip = step1_sfx;
            stepIndex = 1;
        } else if (stepIndex == 1) {
            audio_s.clip = step2_sfx;
            stepIndex = 0;
        }
        audio_s.Play();
    }

    private void SpeedControl() {
        if (OnSlope() && !exitingSlope) {
            if (rb.velocity.magnitude > moveSpeed) {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        } else {    
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump() {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        float jumpMult = 0;
        if (readyToHighJump) {
            jumpMult = highJumpPower;
            readyToHighJump = false;
            highJumpInUse = true;
            StartCoroutine(gameController.fadeoutHighJump());
        } else {
            jumpMult = jumpForce;
        }

        rb.AddForce(transform.up * jumpMult, ForceMode.Impulse);


        jump_audio_player.clip = jump_sfx;
        jump_audio_player.Play();

        if (readyToSlowfall) {    
            readyToSlowfall = false;
            float delay = slowfallDelay;
            if (highJumpInUse) {
                delay += 0.5f;
            }
            StartCoroutine(changeGrav(delay));
        }
    }

    private void ResetJump() {
        canJump = true;
        exitingSlope = false;
    }

    private bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, ground)) {
            
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            // Debug.Log(angle);
            return angle < maxSlopeAngle && angle != 0;
        } else {
            return false;
        }

    }

    private Vector3 GetSlopeMoveDirection() {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }


    public void readySlowfall() {
        readyToSlowfall = true;
        if (grounded) {
            slowfallInUse = false;
        } else {
            StartCoroutine(changeGrav(0.25f));
        }
    }

    private IEnumerator changeGrav(float delay) {
        yield return new WaitForSeconds(delay);
        slowfallInUse = true;
        gravityMultiplier = slowfallMultiplier;
    }

    public void readyHighJump() {
        readyToHighJump = true;
    }

}
