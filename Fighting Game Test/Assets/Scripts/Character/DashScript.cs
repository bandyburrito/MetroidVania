// This 'using' statement imports Unity's coroutine functionality
// Coroutines allow us to spread actions over multiple frames (like waiting for time to pass)
using System.Collections;

// This imports all basic Unity functionality like MonoBehaviour, GameObject, etc.
using UnityEngine;

// CLASS DEFINITION - OOP Principle: Encapsulation
// This class encapsulates (bundles together) all the data and behavior related to player dashing
// 'public' means other scripts can access this class
// ': MonoBehaviour' means this class INHERITS from Unity's MonoBehaviour class
// Inheritance (OOP) - we get all of MonoBehaviour's functionality (like Update, Start, etc.)
public class DashScript : MonoBehaviour
{
    // SERIALIZED FIELDS - Unity Inspector Integration
    // [Header("References")] creates a label in the Unity Inspector for organization
    [Header("References")]
    
    // [SerializeField] exposes this private field to the Unity Inspector
    // OOP Principle: Encapsulation - we keep the field private but allow editing in Inspector
    // This maintains data protection while giving designers control
    // Rigidbody2D handles all physics for 2D objects (velocity, forces, collisions)
    [SerializeField] private Rigidbody2D rb;
    
    // DASH PARAMETERS
    // These are configurable values that define how the dash feels
    [Header("Dash")]
    
    // How fast the player moves during dash (units per second)
    [SerializeField] private float dashSpeed = 15f;
    
    // How long the dash lasts (in seconds)
    [SerializeField] private float dashTime = 0.16f;
    
    // How long to wait before you can dash again (in seconds)
    [SerializeField] private float dashCooldown = 0.35f;
    
    // FEEL PARAMETERS - These affect how the dash "feels" to play
    [Header("Feel")]
    
    // If true, keeps your upward/downward momentum during dash
    // If false, dash flattens your vertical movement (more like Hollow Knight)
    [SerializeField] private bool preserveVerticalVelocity = false;
    
    // How smoothly the dash speed decreases at the end (0 = instant stop, higher = gradual)
    [SerializeField] private float endDamp = 0.15f;
    
    // STATE VARIABLES - OOP Principle: State Management
    // These track the current state of the dash system
    
    // Is the player currently in the middle of a dash?
    private bool isDashing;
    
    // Is the dash off cooldown and ready to use?
    private bool canDash = true;
    
    // Stores the original gravity value so we can restore it after dashing
    private float defaultGravity;
    
    // Which direction is the player facing? (1 = right, -1 = left)
    private int facingDir = 1;
    
    // AWAKE METHOD - Unity Lifecycle
    // Awake() is called once when the GameObject is created, before Start()
    // Use Awake for initialization, especially getting component references
    void Awake()
    {
        // If rb wasn't assigned in Inspector, try to find it on this GameObject
        // GetComponent<T>() searches this GameObject for a component of type T
        // This is defensive programming - prevents null reference errors
        if (!rb) rb = GetComponent<Rigidbody2D>();
        
        // Store the default gravity scale so we can turn it back on after dash
        // During dash, we disable gravity so the player doesn't fall
        defaultGravity = rb.gravityScale;
    }
    
    // UPDATE METHOD - Unity Lifecycle
    // Update() is called every frame (typically 60 times per second)
    // Use Update for input checking and per-frame logic
    void Update()
    {
        // INPUT HANDLING - Read horizontal input
        // GetAxisRaw returns -1 (left), 0 (none), or 1 (right)
        // This reads the keyboard A/D or arrow keys
        float x = Input.GetAxisRaw("Horizontal");
        
        // Update which direction the player is facing based on input
        // 0.01f is a small threshold to avoid floating-point precision issues
        if (x > 0.01f) facingDir = 1;        // Moving right
        else if (x < -0.01f) facingDir = -1;  // Moving left
        // Note: if x is 0, facingDir stays the same (keeps last direction)
        
        // Check if the dash button was pressed THIS frame
        // GetKeyDown only returns true on the first frame the key is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Try to start a dash
            TryDash();
        }
    }
    
    // CUSTOM METHOD - Encapsulation of dash logic
    // This method checks if dashing is allowed, then starts the dash
    void TryDash()
    {
        // Guard clause - exit early if we can't dash
        // Can't dash if: still on cooldown OR already dashing
        if (!canDash || isDashing) return;
        
        // Start the dash coroutine
        // Coroutines allow us to spread actions over multiple frames
        StartCoroutine(DashRoutine());
    }
    
    // COROUTINE - Unity's way of handling time-based actions
    // IEnumerator is the return type for all coroutines
    // Coroutines can "yield" (pause) and resume on later frames
    IEnumerator DashRoutine()
    {
        // PHASE 1: Setup - Disable dashing and modify physics
        
        // Prevent player from dashing again while this dash is active
        canDash = false;
        isDashing = true;
        
        // Disable gravity during dash so player doesn't fall
        // This makes the dash feel more controlled and "floaty" like Hollow Knight
        rb.gravityScale = 0f;
        
        // Decide whether to keep vertical velocity or flatten it
        // Ternary operator: condition ? valueIfTrue : valueIfFalse
        // If preserveVerticalVelocity is true, keep current Y velocity, otherwise set to 0
        float yVel = preserveVerticalVelocity ? rb.linearVelocity.y : 0f;
        
        // PHASE 2: Dash Loop - Apply constant dash velocity
        
        // Track how much time has passed during the dash
        float t = 0f;
        
        // Loop while we're still within dash time
        while (t < dashTime)
        {
            // Set velocity directly (not adding force - immediate control)
            // X velocity = direction * speed (positive for right, negative for left)
            // Y velocity = stored yVel (either preserved or 0)
            // Vector2 is a 2D vector with X and Y components
            rb.linearVelocity = new Vector2(facingDir * dashSpeed, yVel);
            
            // Increase time counter by the time since last frame
            // Time.deltaTime is the time in seconds since the last frame
            t += Time.deltaTime;
            
            // Yield until next frame - this is what makes it a coroutine
            // The loop continues on the next frame, creating smooth movement
            yield return null;
        }
        
        // PHASE 3: Restore Gravity
        
        // Re-enable gravity so player falls normally again
        rb.gravityScale = defaultGravity;
        
        // PHASE 4: Optional Speed Damping (smoothly slow down)
        
        // If endDamp is greater than 0, smoothly reduce horizontal speed
        if (endDamp > 0f)
        {
            // Store the current horizontal velocity at start of damping
            float startX = rb.linearVelocity.x;
            
            // Track how much time has passed during damping
            float blend = 0f;
            
            // Loop for the duration of endDamp
            while (blend < endDamp)
            {
                // Increase blend timer
                blend += Time.deltaTime;
                
                // Calculate interpolation factor (0 to 1)
                // k = 0 at start, k = 1 at end
                float k = blend / endDamp;
                
                // Lerp (Linear Interpolation) smoothly blends between two values
                // Mathf.Lerp(start, end, t) returns a value between start and end
                // As k goes from 0→1, velocity goes from startX→0 smoothly
                rb.linearVelocity = new Vector2(Mathf.Lerp(startX, 0f, k), rb.linearVelocity.y);
                
                // Wait until next frame
                yield return null;
            }
        }
        
        // PHASE 5: Cleanup
        
        // Dash is finished
        isDashing = false;
        
        // Wait for cooldown duration before allowing another dash
        // WaitForSeconds pauses the coroutine for a specific amount of time
        yield return new WaitForSeconds(dashCooldown);
        
        // Cooldown is over, player can dash again
        canDash = true;
    }
}