using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayer : MonoBehaviour, Player
{
    public float horizontalVelocity;
    public float jumpHeight;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public GameObject bulletsParent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // jump mechanic
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    public void OnSwitch()
    {
        DeactivateBullets();
    }

    void DeactivateBullets()
    {
        // Iterate through all child objects of bulletsParent
        foreach (Transform child in bulletsParent.transform)
        {
            // Get the Bullet component
            Bullet bullet = child.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetActive(false);
            }
        }
    }

    void FixedUpdate() 
    {
        // moving left and right
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
    }
}
