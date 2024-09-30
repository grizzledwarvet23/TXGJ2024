using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifetime;

    public float velocity;
    private Rigidbody2D rb;

    public bool enemy;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * velocity;
        StartCoroutine(DieInTime(lifetime));
    }

    void FixedUpdate()
    {
        rb.velocity = transform.right * velocity;
    }
    

    // This method is called when the collider marked as a trigger enters another collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is in the ground layer
         if (other.CompareTag("Ground"))
        {
            Destroy(gameObject); 
        }
        if(enemy)
        {
            if(other.CompareTag("Player"))
            {
                Player player = other.GetComponent<Player>();
                player.TakeDamage(1);

            }
        } else {
            if(other.CompareTag("Enemy"))
            {
                EnemyBasic enemy = other.GetComponent<EnemyBasic>();
                enemy.TakeDamage(1);

            }
        }
    }

    public IEnumerator DieInTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject); // Destroy after a set time if not already destroyed
    }

    
}