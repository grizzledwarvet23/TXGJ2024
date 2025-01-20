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

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter = 0;

    private float airControlForce = 20;
    private float maxAirSpeed = 4.2f;

    private float risingGravity = 2f;
    private float fallingGravity = 3f;


    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public GameObject tempPlatform;

    public Transform platformPosition;

    public float platformCooldown = 1f;

    private bool canPlacePlatform = true;

    private int numPlatformsLeft = 5;

    public TextMeshProUGUI platformCounterText;

    public AudioSource platformCreateSound;

    public GameObject platformShadow;

    public GameObject platformsParent;
    public GameObject[] platformDots;

    private Animator animator;

    private SpriteRenderer spriteRenderer;  // Reference to SpriteRenderer

    private bool playerBeingPushed = false;

    public BoxCollider2D platformPlaceCollider;
    private bool isPlatformPlacerCollidingWithGround = false;

    public AudioSource jumpSound;

    private LevelMetadata levelMetadata;

    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1f;
    public float flashInterval = 0.2f;


    

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get the SpriteRenderer component
        animator = GetComponent<Animator>();
        levelMetadata = GameObject.Find("LevelMetadata").GetComponent<LevelMetadata>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            isPlatformPlacerCollidingWithGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            isPlatformPlacerCollidingWithGround = false;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        //hey wassup    
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravity;
        } else {
            rb.gravityScale = risingGravity;
        }

        bool jumpButton = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0);
        bool abilityButton = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton5);

        if( jumpButton
        && (isGrounded || coyoteTimeCounter > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpSound.Play();
            coyoteTimeCounter = 0;
        }

        else if( (jumpButton || abilityButton)
        
        
         && canPlacePlatform && numPlatformsLeft > 0)
        {
            numPlatformsLeft--;
            
            platformCounterText.text = "x " + numPlatformsLeft;
            // so when we decrement like from 5 platforms we get to 4 platforms. ok set that index to false.
            platformDots[numPlatformsLeft].SetActive(false);

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

    public void setPlayerBeingPushed(bool pushed, float stopInTime) {
        playerBeingPushed = pushed;
        if(pushed == true)
        {
            StartCoroutine(StopPushingPlayer(stopInTime));
        }
    }


    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");

        if(!playerBeingPushed) {
            // if(isGrounded) {
            if(move != 0) {
                rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
                FlipSprite(move);  // Flip the sprite based on the direction of movement
            } else {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            // } 
            // else {
            //     rb.AddForce(new Vector2(move * airControlForce, 0));
            //     if (Mathf.Abs(rb.velocity.x) > maxAirSpeed)
            //     {
            //         rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxAirSpeed, rb.velocity.y);
            //     }
            //     FlipSprite(move);
            // }
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
        GameObject platformInstance = Instantiate(tempPlatform, platformPosition.position, Quaternion.identity, platformsParent.transform);
        platformInstance.GetComponent<PlayerCreatedPlatform>().SetActive(false);

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
        playerBeingPushed = true;
        // Generate a random X direction (either -1 or 1) for left or right
        float choice = Random.Range(-1, 1);
        float randomXDirection = 2;
        if(choice < 0)
        {
            randomXDirection = -2;
        }

        // Set the Y direction to be positive
        float yDirection = 17f;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        // rb.AddForce(new Vector2(randomXDirection, yDirection), ForceMode2D.Impulse); // UNCOMMENT TO BRING BACK X FORCE
        rb.AddForce(new Vector2(0, yDirection), ForceMode2D.Impulse);
        
        StartCoroutine(StopPushingPlayer(0.2f));
    }

    IEnumerator StopPushingPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        playerBeingPushed = false;
    }

    public void OnSwitch() {

        // for (int i = 0; i < healthSegments.Length; i++)
        // {
        //     if(i < health)
        //     {
        //         healthSegments[i].SetActive(true);
        //     }
        //     else
        //     {
        //         healthSegments[i].SetActive(false);
        //     }
        // }
        
        FindObjectOfType<AttackPlayer>().StopAllCoroutines();
        foreach (Transform child in platformsParent.transform)
        {
            child.GetComponent<PlayerCreatedPlatform>().StopAllCoroutines();
            child.GetComponent<PlayerCreatedPlatform>().SetActive(false);
        }


        canPlacePlatform = true;


        numPlatformsLeft = 5 - platformsParent.transform.childCount;


        for(int i = 0; i < numPlatformsLeft; i++)
        {
            platformDots[i].SetActive(true);
        }
        for(int i = numPlatformsLeft; i < 5; i++)
        {
            platformDots[i].SetActive(false);
        }
        platformCounterText.text = "x " + numPlatformsLeft;
    }

    public void TakeDamage(int d)
    {
        if(isInvulnerable) return;

        levelMetadata.TakeDamage(d);
        StartCoroutine(HandleInvulnerability());
    }

    private IEnumerator HandleInvulnerability()
    {
        isInvulnerable = true;

        float elapsedTime = 0f;
        while(elapsedTime < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);

            elapsedTime += flashInterval;
        }
        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    public void Die()
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get the current scene
        SceneManager.LoadScene(currentScene.name); // Reload the current scene
    }





}