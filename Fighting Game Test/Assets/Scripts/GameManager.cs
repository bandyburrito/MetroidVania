using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int MovementSpeed = 5;
    public float JumpForce = 7f;
    public int BackwordMovementSpeed = 3;
    public int DashSpeed = 10;
    public int DashCooldown = 1;
    public Vector2 movement;
    public Rigidbody2D rb;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CharacterMovement()
    {
        float movementX = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(movementX * MovementSpeed, rb.linearVelocity.y);
    }
}
