using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreatedPlatform : MonoBehaviour
{
    // Duration of the shrink effect in seconds
    public float shrinkDuration = 2f;

    // Scale factor to shrink to (e.g., zero for complete disappearance)
    public Vector3 targetScale = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // Start the shrink coroutine when the platform is created
        StartCoroutine(ShrinkAndDestroy());
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

        // Ensure the platform is set to the target scale before destroying
        transform.localScale = targetScale;

        // Destroy the GameObject
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic can be placed here if needed
    }
}