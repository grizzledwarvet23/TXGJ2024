using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if(other.CompareTag("Player"))
        {
            LevelMetadata levelMetadata = FindObjectOfType<LevelMetadata>();
            if(levelMetadata != null)
            {
                levelMetadata.keysCollected++;
                Destroy(gameObject);
            }
            else {
                Debug.LogWarning("LevelMetadata not found in scene!");
            }
        }
    }





}
