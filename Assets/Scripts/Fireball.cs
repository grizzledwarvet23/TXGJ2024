using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifetime;

    public float velocity;
    private Rigidbody2D rb;

    public bool enemy;

    private float originalRight;


    // Start is called before the first frame update
    void Start()
    {
        originalRight = transform.right.x;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * velocity;
        StartCoroutine(DieInTime(lifetime));
    }

    void FixedUpdate()
    {
        // rb.velocity = transform.right * velocity;
        rb.velocity = new Vector2(originalRight * velocity, rb.velocity.y);
        // Rotate the sprite to align with the velocity direction
        Vector2 direction = rb.velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Apply rotation to the sprite (but not the Rigidbody's velocity)
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }
    

    // This method is called when the collider marked as a trigger enters another collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is in the ground layer
         if (other.CompareTag("Ground"))
        {
            // check if that thing has component called PlayerCreatedPlatform:
            if(!enemy) {
                PlayerCreatedPlatform platform = other.GetComponent<PlayerCreatedPlatform>();
                if(platform != null)
                {
                    platform.TakeDamage(1);
                }
            }
            Destroy(gameObject); 
        }
        if(enemy)
        {
            if(other.CompareTag("Player"))
            {
                Player player = other.GetComponent<Player>();
                player.TakeDamage(1);
                Destroy(gameObject);

            }
        } else {
            if(other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.TakeDamage(1);
                Destroy(gameObject);

            }
            else if(other.CompareTag("Wood"))
            {
                Wood wood = other.GetComponent<Wood>();
                wood.SetBurning();
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator DieInTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject); // Destroy after a set time if not already destroyed
    }

    
}