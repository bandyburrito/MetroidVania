using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{

    public List<GameObject> projectileList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            projectileList.Remove(gameObject);
            Destroy(gameObject);
            
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by projectile!");
            projectileList.Remove(gameObject);
            Destroy(gameObject);
            
        }
    }
}
