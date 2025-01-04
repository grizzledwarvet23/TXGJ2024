using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // OnTriggerEnter2D is called when another collider enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Attempt to access the Player script attached to the GameObject
            Player playerScript = other.GetComponent<Player>();
            
            if (playerScript != null)
            {
                // Call the TakeDamage method and pass 1 as the damage amount
                playerScript.TakeDamage(1);
            }
            else
            {
                Debug.LogWarning("No Player script found on the object tagged as Player.");
            }
        }
    }
}
