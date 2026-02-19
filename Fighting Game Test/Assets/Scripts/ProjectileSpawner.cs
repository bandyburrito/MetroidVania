using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    // Separate lists so vertical and horizontal projectiles are tracked independently
    List<GameObject> verticalProjectiles = new List<GameObject>();
    List<GameObject> horizontalProjectiles = new List<GameObject>();

    private const float projectileSpeed = 10f;
    private const float projectileSize = 100f;
    private const float projectileSpacing = 3f;

    void Start()
    {
       Physics2D.IgnoreLayerCollision(6, 7, true);
        SpawnerVertical();
        SpawnerHorizontal();
    }

    void Update()
    {
        // Each method now only touches its own list — no overwriting
        VerticalProjectileMovement();
        HorizontalProjectileMovement();
    }

    public void SpawnerVertical()
    {
        for (int i = -5; i < 3; i++)
        {
            GameObject projectile = new GameObject("VerticalProjectile_" + (i + 5));
            projectile.transform.position = new Vector3(i * projectileSpacing, 10, 0);
            projectile.transform.localScale = Vector3.one * projectileSize;
            projectile.layer = 7; // Set to VerticalProjectiles layer

            SpriteRenderer sr = projectile.AddComponent<SpriteRenderer>();
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            sr.sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
            sr.color = Color.red;
            

            BoxCollider2D boxCol = projectile.AddComponent<BoxCollider2D>();
            boxCol.isTrigger = true; // Set to false for collision detection

            ProjectileCollision col = projectile.AddComponent<ProjectileCollision>();

            // Pass the vertical list — so it removes itself from the right list on collision
            col.projectileList = verticalProjectiles;

            Rigidbody2D rb = projectile.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0f;

            // Add to the vertical list only
            verticalProjectiles.Add(projectile);
        }
    }

    public void SpawnerHorizontal()
    {
        for (int i = 0; i < 2; i++)
        {
            
            GameObject projectile = new GameObject("HorizontalProjectile_" + i);

            
            projectile.transform.position = new Vector3(14, i * projectileSpacing - 1.5f, 0);
            projectile.transform.localScale = Vector3.one * projectileSize;
            projectile.layer = 6; // Set to HorizontalProjectiles layer
            SpriteRenderer sr = projectile.AddComponent<SpriteRenderer>();
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            sr.sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
            sr.color = Color.blue;
            BoxCollider2D boxCol = projectile.AddComponent<BoxCollider2D>();
            boxCol.isTrigger = true; // Set to false for collision detection

            Rigidbody2D rb = projectile.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0f;

            // Add to the horizontal list only
            horizontalProjectiles.Add(projectile);
        }
    }

    public void VerticalProjectileMovement()
    {
        // Only loops through vertical projectiles — moves them down
        for (int i = 0; i < verticalProjectiles.Count; i++)
        {
            Rigidbody2D rb = verticalProjectiles[i].GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.down * projectileSpeed;
        }
    }

    public void HorizontalProjectileMovement()
    {
        // Only loops through horizontal projectiles — moves them left
        for (int i = 0; i < horizontalProjectiles.Count; i++)
        {
            Rigidbody2D rb = horizontalProjectiles[i].GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.left * projectileSpeed ; // Slower horizontal speed for variety
        }
    }
}
