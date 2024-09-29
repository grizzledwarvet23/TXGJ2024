using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifetime;

    public float velocity;
    private Rigidbody2D rb;



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
    }

    public IEnumerator DieInTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject); // Destroy after a set time if not already destroyed
    }

    
}