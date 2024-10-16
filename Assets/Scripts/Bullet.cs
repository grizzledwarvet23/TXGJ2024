using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Bullet : MonoBehaviour
{
    public float speed = 8f; // Speed of the bullet
    public Rigidbody2D rb; // Rigidbody2D component
    private Vector2 direction; // Direction the bullet will move
    public bool active = false; // Controls whether the bullet moves

    void Start()
    {
    }

    void Update()
    {
        // Update bullet velocity based on active status
        if (active)
        {
            rb.velocity = transform.right * speed; // Move the bullet
        }
        else
        {
            rb.velocity = Vector2.zero; // Keep the bullet suspended
        }
    }

    public void SetActive(bool boo)
    {
        if(boo)
        {
            active = true;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the bullet
        }

        if(hitInfo.CompareTag("FinalBoss"))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

}