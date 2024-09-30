using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlatformPlayer : MonoBehaviour, Player
{
    public float horizontalVelocity;
    public float jumpHeight;
    private Rigidbody2D rb;

    private bool isGrounded = false;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public GameObject tempPlatform;

    public Transform platformPosition;
    
    private float pushForce = 15;

    public float platformCooldown = 1f;

    private bool canPlacePlatform = true;

    private int numPlatformsLeft = 5;

    public TextMeshProUGUI platformCounterText;

    public AudioSource platformCreateSound;

    public GameObject platformShadow;

    public GameObject platformsParent;

    int health = 3;

    private Animator animator;

    private SpriteRenderer spriteRenderer;  // Reference to SpriteRenderer

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get the SpriteRenderer component
        animator = GetComponent<Animator>();
    }
    

    // Update is called once per frame
    void Update()
    {
        //hey wassup    
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

        if(Input.GetMouseButtonDown(0) && canPlacePlatform && numPlatformsLeft > 0)
        {
            numPlatformsLeft--;
            platformCounterText.text = "x " + numPlatformsLeft;

            CreatePlatform();
        }

        if(numPlatformsLeft<=0)
        {
            platformShadow.SetActive(false);
        } 
        else
        {
            platformShadow.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");

        if(move != 0) {
            rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
            FlipSprite(move);  // Flip the sprite based on the direction of movement
        }
    }

    // Method to flip the sprite
    void FlipSprite(float move)
    {
        if (move > 0)
        {
            spriteRenderer.flipX = false;  // Face right
        }
        else if (move < 0)
        {
            spriteRenderer.flipX = true;   // Face left
        }
    }

    void CreatePlatform()
    {
        animator.SetBool("CreatingPlatform", true);
        StartCoroutine(doPlatformCooldown());
        Instantiate(tempPlatform, platformPosition.position, Quaternion.identity, platformsParent.transform);

        if(platformCreateSound != null) {
            platformCreateSound.Play();
        }
        PushPlayer();
    }

    public void SetNotPlacingPlatform()
    {
        animator.SetBool("CreatingPlatform", false);
    }

    IEnumerator doPlatformCooldown()
    {
        canPlacePlatform = false;
        yield return new WaitForSeconds(0.5f);
        canPlacePlatform = true;

    }

    void PushPlayer()
    {
        // Generate a random X direction (either -1 or 1) for left or right
        float choice = Random.Range(-1, 1);
        float randomXDirection = 3;
        if(choice < 0)
        {
            randomXDirection = -3;
        }

        // Set the Y direction to be positive
        float yDirection = 2.5f;

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(randomXDirection, yDirection), ForceMode2D.Impulse);
    }

    public void OnSwitch() {
        Debug.Log("we switched!");
        foreach (Transform child in platformsParent.transform)
        {
            Destroy(child.gameObject);
        }
        numPlatformsLeft = 5;
        platformCounterText.text = "x " + numPlatformsLeft;
    }

    public void TakeDamage(int d)
    {
        health -= d;
        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get the current scene
        SceneManager.LoadScene(currentScene.name); // Reload the current scene
    }
}