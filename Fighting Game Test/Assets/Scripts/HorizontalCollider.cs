using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalCollider : MonoBehaviour
{

    public List<GameObject> horizontalProjectiles;
    private float seconds = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WaitForSeconds(seconds));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Killer"))
        {
            horizontalProjectiles.Remove(gameObject);
            Destroy(gameObject);
            
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by projectile!");
            horizontalProjectiles.Remove(gameObject);
            Destroy(gameObject);
            
        }
    }

    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
            BoxCollider2D boxCol = gameObject.AddComponent<BoxCollider2D>();
            ProjectileCollision col = gameObject.AddComponent<ProjectileCollision>();

            // Pass the horizontal list — so it removes itself from the right list on collision
            col.projectileList = horizontalProjectiles;
    }



}
