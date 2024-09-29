using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreatedPlatform : MonoBehaviour
{
    // Duration of the shrink effect in seconds
    public float shrinkDuration = 2f;

    // Scale factor to shrink to (e.g., zero for complete disappearance)
    public Vector3 targetScale = Vector3.zero;

    // Components to disable after shrinking
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to the SpriteRenderer and Collider2D components
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    private IEnumerator ShrinkAndDestroy()
    {
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;

        // Gradually shrink the platform over the specified duration
        while (elapsedTime < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

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

        // Optionally, destroy the object after the effect is completed
        // Destroy(gameObject); // Uncomment if you want to completely destroy the platform object
    }

    public void ShrinkPlatform()
    {
        StartCoroutine(ShrinkAndDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic can be placed here if needed
    }
}