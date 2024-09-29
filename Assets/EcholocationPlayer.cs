using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class EcholocationPlayer : MonoBehaviour, Player
{
    public float horizontalVelocity;
    public float jumpHeight;
    private Rigidbody2D rb;

    private bool isGrounded = false;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    int health = 1;

    public AudioSource echoSound;

    // Reference to the Light2D component in the child object
    private UnityEngine.Rendering.Universal.Light2D echolocationLight;

    // Duration for the echolocation effect
    public float echolocationDuration = 2.0f;

    // Range in which the echolocation can detect monsters
    public float echolocationRange = 10.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Find the Light2D component on the child object
        echolocationLight = GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
    }

    void Update()
    {
        // Ground check logic
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

        // If left mouse button is pressed, trigger the echolocation
        if (Input.GetMouseButtonDown(0))
        {
            DoEcholocation();
        }
    }

    void DoEcholocation()
    {
        echoSound.Play();
        StartCoroutine(EcholocationEffect());

        // Find all objects with the "Monster" tag within a certain range
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        
        foreach (GameObject monsterObj in monsters)
        {
            // Check if the monster is within range
            float distance = Vector2.Distance(transform.position, monsterObj.transform.position);

            if (distance <= echolocationRange)
            {
                // Access the 'Monster' component attached to the GameObject
                Monster monsterComponent = monsterObj.GetComponent<Monster>();

                if (monsterComponent != null)
                {
                    // Do something with the Monster component
                    monsterComponent.Trigger();
                }
            }
        }
    }

    IEnumerator EcholocationEffect()
    {
        // Set the light intensity to 1
        echolocationLight.intensity = 1;

        // Gradually reduce the intensity over the duration
        float elapsedTime = 0;

        while (elapsedTime < echolocationDuration)
        {
            // Calculate the new intensity based on time passed
            echolocationLight.intensity = Mathf.Lerp(1, 0, elapsedTime / echolocationDuration);

            // Increase the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the light intensity is set to 0 at the end
        echolocationLight.intensity = 0;
    }

    void FixedUpdate()
    {
        // Handle horizontal movement
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
    }

    public void TakeDamage(int d)
    {
        health -= d;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); // Reload the current scene
    }

    public void OnSwitch()
    {
        // Implement switching logic here if needed
    }
}