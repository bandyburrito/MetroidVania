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
            
            
        }
    }





}
