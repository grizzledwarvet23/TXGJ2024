using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPlayer : MonoBehaviour, Player
{
    public float horizontalVelocity;
    public float jumpHeight;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public GameObject bulletsParent;

    public Transform firePoint;

    public float firePointDistance = 1.5f;

    private Transform player;

    public AudioSource ricochetSound;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        AimTowardsMouse();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    void AimTowardsMouse()
    {
        // Get the mouse position in the world
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Keep the z-coordinate at 0 for 2D gameplay

        // Calculate the direction from the player to the mouse position
        Vector2 direction = (mousePosition - player.position).normalized;

        // Calculate the angle in degrees to rotate
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Update fire point position based on angle and distance
        firePoint.position = player.position + (Vector3)(direction * firePointDistance);

        // Smoothly rotate the fire point towards the angle
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        firePoint.rotation = targetRotation;
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
    }

    public void OnSwitch()
    {
        ActivateBullets();
    }

    void ActivateBullets()
    {
        // Iterate through all child objects of bulletsParent
        foreach (Transform child in bulletsParent.transform)
        {
            // Get the Bullet component
            Bullet bullet = child.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetActive(true);
            }
        }
    }

    // This method is called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the tag "Bullet"
        if (collision.CompareTag("Bullet"))
        {
            // Get the Rigidbody2D component of the bullet
            Rigidbody2D bulletRb = collision.GetComponent<Rigidbody2D>();

            // Set the bullet's rotation to match the firePoint's rotation
            collision.transform.rotation = firePoint.rotation;

            // Optional: Redirect the bullet's velocity in the direction it is now facing
            if (bulletRb != null)
            {
                bulletRb.velocity = firePoint.up * bulletRb.velocity.magnitude; // Redirect with the same speed
            }

            ricochetSound.Play();
            
        }
    }
}