using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed;          // Speed of the monster

    public float quickSpeed;
    public float smoothFollowFactor = 0.1f; // How smoothly the monster follows the player
    public float inertia = 0.05f;         // Amount of inertia (resistance to changing direction)

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 currentVelocity;      // Stores current velocity to add inertia
    private bool isFacingRight = true;    // Track if the monster is facing right

    public AudioSource screechSound;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Make sure gravity is disabled since the monster is flying
        rb.gravityScale = 0;
    }

    void Update()
    {
        // Continuously find the player in case the player GameObject changes
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // If no player is found, exit early
        if (player == null)
        {
            return;
        }

        // Calculate direction to the player
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // Add inertia to prevent instant direction change
        currentVelocity = Vector2.Lerp(currentVelocity, directionToPlayer * moveSpeed, inertia);

        // Move the monster towards the player smoothly without rotation
        rb.velocity = currentVelocity;

        // Flip the monster's sprite based on the player's position (if moving left or right)
        if ((player.position.x < transform.position.x && isFacingRight) || (player.position.x > transform.position.x && !isFacingRight))
        {
            Flip();
        }
    }

    public void Trigger()
    {
        screechSound.Play();
        StartCoroutine(SpeedUp());
    }

    public IEnumerator SpeedUp()
    {
        float oldSpeed = moveSpeed;
        moveSpeed = quickSpeed;
        yield return new WaitForSeconds(1);
        moveSpeed = oldSpeed;
    }

    // Flip the monster's sprite
    void Flip()
    {
        isFacingRight = !isFacingRight;  // Toggle the facing direction

        // Multiply the x component of localScale by -1 to flip the sprite
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Trigger detection
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        // Check if the object we collided with is tagged as "Player"
        if (collision.CompareTag("Player"))
        {
            // Try to access the Player component on the object
            Player playerComponent = collision.GetComponent<Player>();

            // If the player component exists, call the Die function
            if (playerComponent != null)
            {
                playerComponent.Die();
            }
        }
    }
}