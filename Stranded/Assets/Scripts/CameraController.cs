using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Character References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterTransform;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;
    
    [Header("Camera Settings")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 2f, -5f);
    [SerializeField] private float cameraSmoothSpeed = 10f;
    
    [Header("Game Settings")]
    [SerializeField] private bool foundTreasure = false;
    [SerializeField] private HUD hud;
    [SerializeField] private bool victory = false;

    private Vector3 velocity;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
        
        if (characterTransform == null)
        {
            characterTransform = transform;
        }
    }
    
    void Update()
    {
        if (victory) return;
        HandleMovement();
        HandleCameraFollow();
    }
    
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        
        if (moveDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    private void HandleCameraFollow()
    {
        if (mainCamera == null) return;
        
        Vector3 desiredPosition = characterTransform.position + characterTransform.TransformDirection(cameraOffset);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);
        mainCamera.transform.LookAt(characterTransform.position + Vector3.up * 1.5f);
    }
    
    public void HandleExtendedTriggerEnter(Collider other)
    {
        if (other.GetComponent<Terrain>() != null || other.gameObject.CompareTag("Terrain"))
        {
            return;
        }
        
        Debug.Log($"[CameraController] Collision Entered: {other.gameObject.name}");
        if (other.gameObject.CompareTag("Gold"))
        {
            Debug.Log("[CameraController] collided with Gold object.");
            Destroy(other.transform.root.gameObject);
            foundTreasure = true;
            int goldCoins = Random.Range(12, 500);
            hud.UpdateCoins($"Coins: {goldCoins}");
            hud.UpdateInstructions("Find the boat to escape with your gold coins!");
            Debug.Log("[CameraController] You gained 100 gold pieces.");
        }

        if (other.gameObject.CompareTag("Boat"))
        {
            if (foundTreasure == true){
                hud.SetVictory("Congratulations you have escaped alive with your gold coins!");
                victory = true;
            }
        }
    }
    
    public void HandleExtendedTriggerExit(Collider other)
    {
        if (other.GetComponent<Terrain>() != null || other.gameObject.CompareTag("Terrain"))
        {
            return;
        }
        
        Debug.Log($"[CameraController] Collision Exited: {other.gameObject.name}");
    }
}