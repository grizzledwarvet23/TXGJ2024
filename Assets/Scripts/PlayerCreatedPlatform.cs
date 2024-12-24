using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreatedPlatform : MonoBehaviour
{
    // Duration of the shrink effect in seconds

    // if platform gets hit by fire twice, then it dies too.
    private int platformHealth = 2; 
    public float shrinkDuration = 2f;

    // Scale factor to shrink to (e.g., zero for complete disappearance)
    public Vector3 targetScale = Vector3.zero;

    // Components to disable after shrinking
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;

    public Sprite burnedSprite;

    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to the SpriteRenderer and Collider2D components
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damage)
    {
        platformHealth -= damage;
        if(platformHealth == 1)
        {
            //set sprite to burned sprite:
            spriteRenderer.sprite = burnedSprite;
        }
        if(platformHealth <= 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    //whether to make it collidable or nah.
    public void SetActive(bool active)
    {
        Start();
        if(active)
        {
            collider2D.enabled = true;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        } 
        else //inactive time
        {
            collider2D.enabled = false;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
        }
    }

    public IEnumerator ShrinkPlatform()
    {
        Vector3 originalScale = transform.localScale;

        // Gradually shrink the platform over the specified duration
        while (elapsedTime < shrinkDuration)
        {
            if(this == null || gameObject == null)
            {
                yield break;
            }
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        if(this != null && gameObject != null)
        { 
            // Ensure the platform is set to the target scale before finishing
            transform.localScale = targetScale;

            // Disable the sprite renderer and collider
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false; // Disables the sprite
            }

            if (collider2D != null)
            {
                collider2D.enabled = false; // Disables the collider
            }
            
            Destroy(gameObject);
        }
        // Optionally, destroy the object after the effect is completed
        // Destroy(gameObject); // Uncomment if you want to completely destroy the platform object
    }
}