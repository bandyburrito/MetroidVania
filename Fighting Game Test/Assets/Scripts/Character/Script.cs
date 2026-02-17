using UnityEngine;

public class Script : MonoBehaviour
{
    // === MOVEMENT SETTINGS ===
    // [SerializeField] allows you to edit this private variable in the Unity Inspector
    // This follows the Encapsulation principle (OOP) - keep variables private but allow controlled access
    [SerializeField] private float moveSpeed = 5f;
    
    // Private variable to store the horizontal input value (-1, 0, or 1)
    private float horizontalInput;
    private float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update() is called every frame by Unity's game loop
    // This is where we handle input detection and character movement
    void Update()
    {
        // Step 1: Check for player keyboard input
        HandleInput();
        
        // Step 2: Move the character based on the input we received
        MoveCharacter();

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("SPACE KEY PRESSED - JUMP!");
            isGrounded = false;
        }
    }

    // Method (function) that detects keyboard input
    // Using methods separates different responsibilities (Single Responsibility Principle - OOP)
    private void HandleInput()
    {
        // Input.GetAxis() is part of Unity's OLD Input System
        // "Horizontal" is a built-in axis configured in Edit > Project Settings > Input Manager
        // Returns: -1 when pressing A/Left Arrow, 0 when nothing pressed, 1 when pressing D/Right Arrow
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Optional: Debug log to verify input is working
        // Shows in the Console window when you press movement keys

    }

    // Method that handles the actual movement of the GameObject
    // Transform component controls the GameObject's position, rotation, and scale
    private void MoveCharacter()
    {
        // Calculate how much to move THIS frame
        // horizontalInput: direction (-1 left, 1 right)
        // moveSpeed: units per second
        // Time.deltaTime: time since last frame (makes movement frame-rate independent)
        // Example: 1 * 5 * 0.016 = 0.08 units moved this frame at 60fps
        float moveAmount = horizontalInput * moveSpeed * Time.deltaTime;
        
        // Transform.Translate() moves the GameObject by a given direction
        // Vector3.right is Unity's shorthand for (1, 0, 0) - the right direction in 3D space
        // We multiply it by moveAmount to scale the movement
        // This moves the object directly without using physics (Rigidbody)
        transform.Translate(Vector3.right * moveAmount);
        
        // Optional: Debug log to see movement happening and track position
        if (moveAmount != 0)
        {
            Debug.Log("MOVING: " + moveAmount + " units. Current position: " + transform.position);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}

