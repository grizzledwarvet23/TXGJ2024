using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.Tilemaps;

public class EcholocationPlayer : MonoBehaviour, Player
{
    public float horizontalVelocity;
    public float jumpHeight;
    private Rigidbody2D rb;


    public GameObject echoBG;
    public GameObject regularBG;


    private bool isGrounded = false;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    int health = 1;

    public AudioSource echoSound;

    public GameObject regularTilemap;
    public GameObject echolocationTilemap;

    // Reference to the Light2D component in the child object
    private UnityEngine.Rendering.Universal.Light2D echolocationLight;

    // Duration for the echolocation effect
    public float echolocationDuration = 2.0f;

    // Range in which the echolocation can detect monsters
    public float echolocationRange = 10.0f;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private bool canEcholocate;

    void Start()
    {
        canEcholocate = true;
        rb = GetComponent<Rigidbody2D>();
        // Find the Light2D component on the child object
        echolocationLight = GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Ground check logic
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

        if(Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("moving", true);
        } else {
            animator.SetBool("moving", false);
        }

        // If left mouse button is pressed, trigger the echolocation
        if (Input.GetMouseButtonDown(0) && canEcholocate)
        {
            DoEcholocation();
        }

        
    }

    IEnumerator SetNoEchoAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("echo", false);
    }

    void DoEcholocation()
    {
        canEcholocate = false;
        animator.SetBool("echo", true);
        StartCoroutine(SetNoEchoAnimation());
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
        // echolocationLight.intensity = 1;
        // Get the Tilemap and TilemapRenderer components of the echolocationTilemap
        Tilemap echolocationTilemapComponent = echolocationTilemap.GetComponent<Tilemap>();
        Tilemap echolocationTilemapRenderer = echolocationTilemap.GetComponent<Tilemap>();

        // Set the starting color of the tilemap to white
        echolocationTilemapRenderer.color = Color.white;

        // Duration of the echolocation effect
        float elapsedTime = 0;

        // Gradually reduce the light intensity and tilemap color over the duration
        while (elapsedTime < echolocationDuration)
        {
            // Calculate the new intensity for the light based on time passed
            echolocationLight.intensity = Mathf.Lerp(1, 0, elapsedTime / echolocationDuration);

            // Calculate the color from white to black based on time passed
            Color newColor = Color.Lerp(Color.white, Color.black, elapsedTime / echolocationDuration);
            echolocationTilemapRenderer.color = newColor;

            // Increase the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the light intensity and tilemap color are set to final values at the end
        echolocationLight.intensity = 0;
        echolocationTilemapRenderer.color = Color.black;
        canEcholocate = true;
    }

    // IEnumerator EcholocationEffect()
    // {
    //     // Set the light intensity to 1
    //     echolocationLight.intensity = 1;

    //     // Gradually reduce the intensity over the duration
    //     float elapsedTime = 0;

    //     while (elapsedTime < echolocationDuration)
    //     {
    //         // Calculate the new intensity based on time passed
    //         echolocationLight.intensity = Mathf.Lerp(1, 0, elapsedTime / echolocationDuration);

    //         // Increase the elapsed time
    //         elapsedTime += Time.deltaTime;

    //         // Wait for the next frame
    //         yield return null;
    //     }

    //     // Ensure the light intensity is set to 0 at the end
    //     echolocationLight.intensity = 0;
    // }



    void FixedUpdate()
    {
        // Handle horizontal movement
        float move = Input.GetAxis("Horizontal");
        if(move != 0) {
            rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
            FlipSprite(move);
        }
    }

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
        canEcholocate = true;
        Tilemap echolocationTilemapRenderer = echolocationTilemap.GetComponent<Tilemap>();

        // Implement switching logic here if needed
        echolocationTilemap.SetActive(true);
        regularTilemap.SetActive(false);

        echoBG.SetActive(true);
        regularBG.SetActive(false);
        if(echolocationLight == null) {
            echolocationLight= GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
        }
        echolocationLight.intensity = 0;
        echolocationTilemapRenderer.color = Color.black;
        

    }
}