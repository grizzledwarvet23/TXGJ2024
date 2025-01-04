using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackPlayer : MonoBehaviour, Player
{
    public float horizontalVelocity;
    public float jumpHeight;
    private Rigidbody2D rb;

    private bool isGrounded = false;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter = 0;

    private float risingGravity = 2f;
    private float fallingGravity = 3f;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public GameObject projectile;

    public float projectileSpeed;

    public Transform firePoint;

    public GameObject platformsParent;

    private SpriteRenderer spriteRenderer;

    private Animator animator;

    int health = 1;
    public GameObject[] healthSegments;

    private bool canAttack = true;

    public AudioSource attackSound;

    void Start() {
        canAttack = true;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

        if(Input.GetKeyDown(KeyCode.Space) && (isGrounded || coyoteTimeCounter > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            coyoteTimeCounter = 0;
        }

        if(rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravity;
        } else {
            rb.gravityScale = risingGravity;
        }



        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - transform.position).normalized;
        firePoint.position = (Vector2) transform.position + direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if(Input.GetMouseButtonDown(0) && canAttack)
        {
            ShootProjectile();
        }

        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        // }
    }

    void FixedUpdate()
    {
        // we do movement:
        float move = Input.GetAxis("Horizontal");
        if(move != 0) {
            rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
            FlipSprite(move);  // Flip the sprite based on the direction of movement
        } else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

    }

    IEnumerator SetNotAttacking()
    {
        //one for animation, one for attack boolean.
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
        
    }

    void ShootProjectile()
    {
        canAttack = false;
        animator.SetBool("attacking", true);
        StartCoroutine(SetNotAttacking());
        GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        attackSound.Play();
        Rigidbody2D projectileRb = newProjectile.GetComponent<Rigidbody2D>();

        // projectileRb.velocity = direction * projectileSpeed;

        

        // // Get the mouse position in world space
        // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // // Calculate the direction from the player to the mouse position
        // Vector2 direction = (mousePosition - transform.position).normalized;
        // direction.Normalize();
        // // .normalized;

        // // Set the projectile's velocity based on the direction and speed
    }

    public void TakeDamage(int d)
    {
        health -= d;

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

    public void OnSwitch() 
    {
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

        
        canAttack = true;
        Transform parentTransform = platformsParent.transform;
        foreach(Transform child in platformsParent.transform)
        {
            child.gameObject.GetComponent<PlayerCreatedPlatform>().SetActive(true);
        }
        StartCoroutine(ShrinkPlatformsSequentially(parentTransform));
    }

private IEnumerator ShrinkPlatformsSequentially(Transform parentTransform)
{
    // we are gonna redesign this, basically just shrink out whatever is first in this list, if its not empty.

    if(parentTransform.childCount > 0)
    {
        Transform platform = parentTransform.GetChild(0);
        PlayerCreatedPlatform platformComponent = platform.GetComponent<PlayerCreatedPlatform>();
        yield return StartCoroutine(platformComponent.ShrinkPlatform());
        StartCoroutine(ShrinkPlatformsSequentially(parentTransform));
    }

    // Loop through all children of platformsParent
    // int count = 0;
    // for (int i = 0; i < parentTransform.childCount; i++)
    // {
    //     Transform platform = parentTransform.GetChild(i);
    //     count++;

    //     // Get the PlayerCreatedPlatform component from the child platform
    //     PlayerCreatedPlatform platformComponent = platform.GetComponent<PlayerCreatedPlatform>();
        
    //     // Assume the component exists and directly call ShrinkPlatform method
    //     if (platformComponent != null)
    //     {
    //         platformComponent.ShrinkPlatform();
            
    //         // Wait for 2 seconds before shrinking the next platform
    //         yield return new WaitForSeconds(2f);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Platform component not found on " + platform.name);
    //     }
    // }
}
}
