using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    // Reference to the kickbox prefab that will be instantiated
    public GameObject KickBox;
    
    // Empty GameObjects that mark where kicks should spawn (left/right of player)
    public GameObject LeftKick;
    public GameObject RightKick;
    public GameObject UpKick;
    private float attackColldown = 0.5f;
    private bool canAttack = true;
    private float RecoilForce = 3f;
    
    
    // How long the attack hitbox stays active before being destroyed
    [SerializeField] private float attackDuration = 0.3f;

    // Update is called once per frame - checks for attack input
    void Update()
    {
        // Check if this GameObject has the "Player" tag (good practice for multiplayer/AI)
        if (gameObject.CompareTag("Player"))
        {
            // Check if K is pressed DOWN this frame AND D is being held
            // This triggers a right kick
            if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.D) && canAttack)    
            {
                // Instantiate creates a new GameObject from the KickBox prefab
                // Position: at the RightKick marker's position
                // Rotation: Quaternion.identity means no rotation (0, 0, 0)
                GameObject instantiatedKickBox = Instantiate(KickBox, RightKick.transform.position, Quaternion.identity);
                
                // NEW: Make the instantiated kickbox a CHILD of the RightKick marker
                // This means it will follow the player's movement because RightKick follows the player
                instantiatedKickBox.transform.SetParent(RightKick.transform);
                
                // Start the coroutine that will destroy this hitbox after attackDuration seconds
                StartCoroutine(AttackDuration(instantiatedKickBox));
                StartCoroutine(AttackCooldown());

            }
            
            // Check if K is pressed DOWN this frame AND A is being held
            // This triggers a left kick
            if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.A) && canAttack)
            {
                // Same as above but for left side
                GameObject instantiatedKickBox = Instantiate(KickBox, LeftKick.transform.position, Quaternion.identity);
                
                // NEW: Make it a child of LeftKick so it follows the player
                instantiatedKickBox.transform.SetParent(LeftKick.transform);
                
                StartCoroutine(AttackDuration(instantiatedKickBox));
                StartCoroutine(AttackCooldown());
            }

            if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.W) && canAttack)
            {
                GameObject instantiatedKickBox = Instantiate(KickBox, UpKick.transform.position, Quaternion.Euler(0f, 0f, 90f));
                instantiatedKickBox.transform.SetParent(UpKick.transform);
                StartCoroutine(AttackDuration(instantiatedKickBox));
                StartCoroutine(AttackCooldown());
            }
        }
    }

    // Coroutine = special method that can pause execution and resume later
    // Used here to wait for attackDuration seconds before destroying the hitbox
    IEnumerator AttackDuration(GameObject attackHitbox)
    {
        // yield return pauses this coroutine for the specified time
        // The rest of the game keeps running during this wait
        yield return new WaitForSeconds(attackDuration);
        
        // After waiting, destroy the attack hitbox GameObject
        // Use Destroy() instead of DestroyImmediate() - safer and recommended
        Destroy(attackHitbox);
    }

    IEnumerator AttackCooldown()
    {
        // Prevent attacking again until cooldown is over
        canAttack = false;
        
        // Wait for the cooldown duration
        yield return new WaitForSeconds(attackColldown);
        
        // Allow attacking again
        canAttack = true;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 currentpoisition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y );
        }   
    } 
}
