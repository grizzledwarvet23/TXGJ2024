using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour, Enemy
{
    // Public Variables
    public float speed = 2f; // Speed of the enemy's patrol
    public float patrolDistance = 5f; // How far the enemy patrols
    public float detectionRange;     // Range to detect the player
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

    private int health = 2;

    public AudioSource shootSound;

    private SpriteRenderer spriteRenderer;

    private Material originalMat;
    public Material whiteFlashMat;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMat = spriteRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        DetectAndShootPlayer();
    }

    public void TakeDamage(int d)
    {
        health -= d;

        Debug.Log("Gonna flash...");
        StartCoroutine(FlashWhite());

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FlashWhite()
    {
        Debug.Log("Flashing white");
        spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds
        spriteRenderer.material = originalMat;
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
        // Check for players within the circular detection range
        Collider2D[] playersInRange = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);

        // If a player is detected and it's time to shoot
        foreach (Collider2D player in playersInRange)
        {
            if (player.CompareTag("Player") && Time.time > nextFireTime)
            {
                AimAtPlayer(player.transform);
                Shoot();
                nextFireTime = Time.time + fireRate; // Set the next time the enemy can fire
                break; // Only shoot once per update
            }
        }
    }

    void AimAtPlayer(Transform player)
    {
        // Calculate the direction to the player
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Calculate the angle to rotate the firePoint
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the firePoint towards the player
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }   


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(1);
        }
    }

    void Shoot()
    {
        // Instantiate a projectile at the firePoint
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        shootSound.Play();
    }

    // Gizmos to visualize the detection range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Color for the detection range gizmo
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Draw the circular range
    }
}