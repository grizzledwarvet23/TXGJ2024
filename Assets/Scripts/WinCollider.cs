using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCollider : MonoBehaviour
{
    public string sceneToLoad;

    public bool requiresKey = false;
    public int keysRequired = 1;

    private LevelMetadata levelMetadata;

    void Start()
    {
        levelMetadata = FindObjectOfType<LevelMetadata>();
    }

    // This function is called when the Collider2D other enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that triggered the collider has the tag "Player"
        if (other.CompareTag("Player") &&
            (!requiresKey || 
            levelMetadata.keysCollected >= keysRequired))
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
