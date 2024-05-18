using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    private float speedMultiplier;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    
    private bool canJump;
    public Transform orientation;

    private ConstantForce cForce;
    public float gravityMultiplier;
    private Vector3 gravDirection;
    private bool sprinting;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    private Rigidbody rb;
    

    [Header("Light Powers")]
    public GameController gameController;
    public bool readyToSlowfall;
    public bool slowfallInUse;
    public float slowfallMultiplier;
    public float slowfallDelay;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    public bool grounded;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cForce = GetComponent<ConstantForce>();
        gravDirection = new Vector3(0, -9.81f, 0);
        cForce.force = gravDirection * gravityMultiplier;
        rb.freezeRotation = true;
        canJump = true;
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(jumpKey) && grounded && canJump) {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if(Input.GetKey(KeyCode.LeftShift)) {
            sprinting = true;
            speedMultiplier = sprintSpeed;
        } else {
            sprinting = false;
            speedMultiplier = walkSpeed;
        }
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded) {
            rb.AddForce(moveDirection.normalized * speedMultiplier * 10f, ForceMode.Force);
        } else {
            rb.AddForce(moveDirection.normalized * speedMultiplier * 10f * airMultiplier, ForceMode.Force);
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
        MyInput();
        if (grounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0;
        }
        SpeedControl();
    }

    private void FixedUpdate() {
        MovePlayer();
        if (!slowfallInUse) {
            gravityMultiplier = 3;
        }
        cForce.force = gravDirection * gravityMultiplier;
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > speedMultiplier) {
            Vector3 limitedVel = flatVel.normalized * speedMultiplier;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        if (readyToSlowfall) {
            
            readyToSlowfall = false;
            StartCoroutine(changeGrav(slowfallDelay));
        }
    }

    private void ResetJump() {
        canJump = true;
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

}
