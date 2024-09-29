using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EcholocationPlayer : MonoBehaviour, Player
{
    public float horizontalVelocity;
    public float jumpHeight;
    private Rigidbody2D rb;

    private bool isGrounded = false;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;


    int health = 1;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    

    // Update is called once per frame
    void Update()
    {
        //hey wassup    
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }



        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(Input.GetMouseButtonDown(0))
        {
            DoEcholocation();
        }

        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        // }
    }

    void DoEcholocation()
    {
        
    }

    void FixedUpdate()
    {
        // we do movement:
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);

    }


    public void TakeDamage(int d)
    {
        health -= d;
        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get the current scene
        SceneManager.LoadScene(currentScene.name); // Reload the current scene
    }

    public void OnSwitch() {
        
    }
}
