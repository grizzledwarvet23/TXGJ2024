using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayer : MonoBehaviour, Player
{
    public float horizontalVelocity;
    public float jumpHeight;
    private Rigidbody2D rb;

    private bool isGrounded = false;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public GameObject tempPlatform;

    public Transform platformPosition;
    
    private float pushForce = 15;

    public float platformCooldown = 1f;

    private bool canPlacePlatform = true;



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

        if(Input.GetMouseButtonDown(0) && canPlacePlatform)
        {
            CreatePlatform();
        }
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        if(move != 0) {
            rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
        }
    }

    void CreatePlatform()
    {
        StartCoroutine(doPlatformCooldown());
        Instantiate(tempPlatform, platformPosition.position, Quaternion.identity);
        PushPlayer();
    }

    IEnumerator doPlatformCooldown()
    {
        canPlacePlatform = false;
        yield return new WaitForSeconds(0.5f);
        canPlacePlatform = true;

    }

    void PushPlayer()
    {
        // Generate a random X direction (either -1 or 1) for left or right
        float randomXDirection = Random.Range(-0.25f, 0.25f);

        // Set the Y direction to be positive
        float yDirection = 0.1f;

        // Create a vector for the force
        Vector2 pushDirection = new Vector2(randomXDirection, yDirection).normalized;
        pushDirection.Normalize();

        // Apply force to the player's Rigidbody in that direction
        rb.velocity = Vector2.zero;
        rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
    }
}
