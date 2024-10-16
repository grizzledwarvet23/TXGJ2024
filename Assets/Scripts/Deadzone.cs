using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deadzone : MonoBehaviour, Player
{


    // Ensure the player's GameObject has the "Player" tag
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the deadzone is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Get the Player script component from the collided object
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Die();
            }
        }
    }

    public void Die()
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get the current scene
        SceneManager.LoadScene(currentScene.name); // Reload the current scene
    }
}