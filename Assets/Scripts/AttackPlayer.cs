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

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public GameObject projectile;

    public float projectileSpeed;

    public Transform firePoint;

    public GameObject platformsParent;

    private SpriteRenderer spriteRenderer;

    private Animator animator;

    int health = 3;

    void Start() {
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

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }



        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - transform.position).normalized;
        firePoint.position = (Vector2) transform.position + direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if(Input.GetMouseButtonDown(0))
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
        }

    }

    IEnumerator SetNotAttacking()
    {
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("attacking", false);
        
    }

    void ShootProjectile()
    {
        animator.SetBool("attacking", true);
        StartCoroutine(SetNotAttacking());
        Debug.Log(firePoint.rotation);
        GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRb = newProjectile.GetComponent<Rigidbody2D>();

        // projectileRb.velocity = direction * projectileSpeed;

        

        // // Get the mouse position in world space
        // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // // Calculate the direction from the player to the mouse position
        // Vector2 direction = (mousePosition - transform.position).normalized;
        // direction.Normalize();
        // // .normalized;

        // // Set the projectile's velocity based on the direction and speed
        // Debug.Log(direction);
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

    public void OnSwitch() 
{
    Transform parentTransform = platformsParent.transform;
    StartCoroutine(ShrinkPlatformsSequentially(parentTransform));
}

private IEnumerator ShrinkPlatformsSequentially(Transform parentTransform)
{
    // Loop through all children of platformsParent
    int count = 0;
    for (int i = 0; i < parentTransform.childCount; i++)
    {
        Transform platform = parentTransform.GetChild(i);
        count++;
        Debug.Log(platform.name + " " + count);

        // Get the PlayerCreatedPlatform component from the child platform
        PlayerCreatedPlatform platformComponent = platform.GetComponent<PlayerCreatedPlatform>();
        
        // Assume the component exists and directly call ShrinkPlatform method
        if (platformComponent != null)
        {
            platformComponent.ShrinkPlatform();
            
            // Wait for 2 seconds before shrinking the next platform
            yield return new WaitForSeconds(2f);
        }
        else
        {
            Debug.LogWarning("Platform component not found on " + platform.name);
        }
    }
}
}
