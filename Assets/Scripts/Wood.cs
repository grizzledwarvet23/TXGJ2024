using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    private Collider2D collider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBurning()
    {
        // Start the burning effect and destruction process
        StartCoroutine(BurningEffect(1.5f));
        StartCoroutine(DieInTime(1.5f));
    }

    IEnumerator BurningEffect(float duration)
    {
        float elapsed = 0f;
        Color initialColor = spriteRenderer.color;
        Color targetColor = Color.red; // Target red color

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }

        // Ensure the final color is set
        spriteRenderer.color = targetColor;
    }

    IEnumerator DieInTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
