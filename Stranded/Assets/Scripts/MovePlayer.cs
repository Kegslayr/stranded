using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] InputAction sprint;
    [SerializeField] InputAction jump;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float rotationSpeed = 5f;
    
    Rigidbody rb;
    Vector3 movementDirection;
    private bool isJumping = false;
    private bool jumpRequested = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable(){
        sprint.Enable();
        jump.Enable();
        jump.performed += OnJumpPerformed;
    }

    private void OnDisable(){
        jump.performed -= OnJumpPerformed;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        HandleRotation();
    }

    void HandleMovement()
    {
        if (ProcessSprint()){
            speed = 5.0f;
        }
        else{
            speed = 3.0f;
        }

        float speedFactor = speed * Time.deltaTime;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        // Store movement direction for rotation
        movementDirection = new Vector3(x, 0, z).normalized;
        
        float y = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            y = 1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            y = -1;
        }

        Vector3 movement = new Vector3(x, y, z) * speedFactor;
        transform.Translate(movement, Space.World);
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpRequested = true;
        }
    }

    void HandleJump()
    {
        // Check if we're grounded and not already jumping
        if (jumpRequested && IsGrounded() && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            jumpRequested = false;
        }
        
        // Reset jumping flag when we land
        if (isJumping && rb.linearVelocity.y <= 0 && IsGrounded())
        {
            isJumping = false;
        }
    }

    void HandleRotation()
    {
        // Only rotate if there's significant movement input
        if (movementDirection.magnitude > 0.1f)
        {
            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(-movementDirection);
            
            // Smoothly rotate towards target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private bool ProcessSprint(){
        return sprint.IsPressed();
    }
}