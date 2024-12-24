using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpotlightPlayer : MonoBehaviour, Player
{
    public float horizontalVelocity;
    public float jumpHeight;
    private Rigidbody2D rb;

    private bool isGrounded = false;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private float pushForce = 15;

    int health = 1;

    public GameObject regularTilemap;
    public GameObject echolocationTilemap;

    public GameObject echoBG;
    public GameObject regularBG;


    private SpriteRenderer spriteRenderer;

    private Animator animator;


    public GameObject spotLight;
    public GameObject focusedLight;



    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // if spotlight is not found, try to see if theres a child called SpotLight and use that:
        if(spotLight == null) {
            spotLight = transform.Find("SpotLight").gameObject;
        }
        // same with focusedlight, with a child called FocusedLight:
        if(focusedLight == null) {
            focusedLight = transform.Find("FocusedLight").gameObject;
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

        if(Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("moving", true);
        } else {
            animator.SetBool("moving", false);
        }


        if (Input.GetMouseButtonDown(0))
        {
            focusedLight.SetActive(true);
            spotLight.SetActive(false);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            focusedLight.SetActive(false);
            spotLight.SetActive(true);
        }

        // if focused light is active, make sure that it is rotated towards the mouse:
        if(focusedLight.activeSelf)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            focusedLight.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        if(move != 0) {
            rb.velocity = new Vector2(move * horizontalVelocity, rb.velocity.y);
            FlipSprite(move);
        } else {
            rb.velocity = new Vector2(0, rb.velocity.y);
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
        if(health <= 0)
        {
            Die();
        }
    }

    public void OnSwitch() {
        echolocationTilemap.SetActive(false);
        regularTilemap.SetActive(true);

        echoBG.SetActive(false);
        regularBG.SetActive(true);

        spotLight.SetActive(true);
        focusedLight.SetActive(false);
    }

    public void Die()
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get the current scene
        SceneManager.LoadScene(currentScene.name); // Reload the current scene
    }

}
