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

        if(Input.GetKeyDown(KeyCode.E))
        {
            CreatePlatform();
        }
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
    }

    void CreatePlatform()
    {
        Instantiate(tempPlatform, platformPosition.position, Quaternion.identity);
    }
}
