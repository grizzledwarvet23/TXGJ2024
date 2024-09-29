using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    // Public Variables
    public float speed = 2f; // Speed of the enemy's patrol
    public float patrolDistance = 5f; // How far the enemy patrols
    public float detectionRange = 10f; // Range to detect the player
    public GameObject projectilePrefab; // Projectile to shoot
    public float fireRate = 1f; // Time between shots
    public Transform firePoint; // Where to instantiate the projectile
    public LayerMask playerLayer; // Layer mask to detect player
    
    // Private Variables
    private bool movingRight = true;
    private Vector2 startingPosition;
    private float nextFireTime = 0f;

    // Rigidbody for movement
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        DetectAndShootPlayer();
    }

    void Patrol()
    {
        // Move left or right
        rb.velocity = new Vector2((movingRight ? 1 : -1) * speed, rb.velocity.y);

        // Check patrol distance
        if (Vector2.Distance(startingPosition, transform.position) >= patrolDistance)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Flip the enemy sprite
        transform.localScale = localScale;
    }

    void DetectAndShootPlayer()
    {
        // Cast a ray in the direction the enemy is facing to detect the player
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, playerLayer);

        // If the player is detected and it's time to shoot
        if (hit.collider != null && hit.collider.CompareTag("Player") && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Set the next time the enemy can fire
        }
    }

    void Shoot()
    {
        // Instantiate a projectile at the firePoint
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}