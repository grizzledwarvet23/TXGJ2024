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

    // new echolocation type shit.


    public Transform echodotsParent;
    public GameObject whiteDotPrefab;
    public int numberOfRaycasts = 360;
    public float rayLength = 50f;
    public LayerMask colliderMask;



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
            StartCoroutine(DoEcholocation());
        }

        
    }

    IEnumerator SetNoEchoAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("echo", false);
    }

    void ShootRaycasts()
    {

        foreach (Transform child in echodotsParent)
        {
            Destroy(child.gameObject);
        }

        float angleStep = 360f / numberOfRaycasts;

        for (int i = 0; i < numberOfRaycasts; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = AngleToVector2(angle);
            
            // Cast the ray
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, colliderMask);

            if (hit.collider != null)
            {
                //CHECK TILEMAP SHIT!
                Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
                if(tilemap != null)
                {
                    // Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
                    // Debug.Log($"Cell Position: {cellPosition}");
                    // if (tilemap.HasTile(cellPosition))
                    // {
                    //     Debug.Log("YES WE DO GOT A TILE!");
                    //     echolocationTilemap.GetComponent<Tilemap>().SetColor(cellPosition, Color.red);
                    //     Color tileColor = tilemap.GetColor(cellPosition);
                    //     Debug.Log($"Tile color after change: {tileColor}");
                    // }
                }


                // Instantiate a white dot at the hit point
                InstantiateWhiteDot(hit.point);
            }
        }
    }

    Vector2 AngleToVector2(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    void InstantiateWhiteDot(Vector2 position)
    {
        if (whiteDotPrefab != null)
        {
            GameObject instantiation = Instantiate(whiteDotPrefab, position, Quaternion.identity);
            instantiation.transform.parent = echodotsParent;
        }
    }

    //used to just be a void function.
    IEnumerator DoEcholocation()
    {
        canEcholocate = false;
        animator.SetBool("echo", true);
        StartCoroutine(SetNoEchoAnimation());
        echoSound.Play();
        yield return StartCoroutine(EcholocationEffect());
        ShootRaycasts();
        TriggerMonster();
    }

    void TriggerMonster()
    {
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
        // NEW EFFECT:
        // black center goes from 0.75 -> 0.95
        // light2D goes from 1 -> 37.
        // lets do this through course of 1.5 seconds.
        // use echolocationObject as the light2D, and use its only child as the "black center":
        //we are only chanigng the scale of light2D and the scale of the black center, not the intensity of the light:
        GameObject echolocationLightObject = echolocationLight.gameObject;
        Transform blackCenter = echolocationLightObject.transform.GetChild(0);
        echolocationLight.intensity = 1;
        blackCenter.gameObject.SetActive(true);
        float elapsedTime = 0;
        while (elapsedTime < echolocationDuration)
        {
            // Calculate the new intensity for the light based on time passed
            float newLightScale = Mathf.Lerp(1, 41, elapsedTime / echolocationDuration);
            echolocationLightObject.transform.localScale = new Vector3(newLightScale, newLightScale, 1);
            // Calculate the scale of the black center based on time passed
            float newBlackScale = Mathf.Lerp(0.75f, 0.93f, elapsedTime / echolocationDuration);
            blackCenter.localScale = new Vector3(newBlackScale, newBlackScale, 1);
            // Increase the elapsed time
            elapsedTime += Time.deltaTime;
            // Wait for the next frame
            yield return null;
        }
        blackCenter.localScale = new Vector3(0.75f, 0.75f, 1);
        blackCenter.gameObject.SetActive(false);
        echolocationLightObject.transform.localScale = new Vector3(1, 1, 1);
        echolocationLight.intensity = 0;






        //OLD EFFECT:
        // Tilemap echolocationTilemapComponent = echolocationTilemap.GetComponent<Tilemap>();
        // Tilemap echolocationTilemapRenderer = echolocationTilemap.GetComponent<Tilemap>();
        // // Set the starting color of the tilemap to white
        // echolocationTilemapRenderer.color = Color.white;
        // // Duration of the echolocation effect
        // float elapsedTime = 0;
        // // Gradually reduce the light intensity and tilemap color over the duration
        // while (elapsedTime < echolocationDuration)
        // {
        //     // Calculate the new intensity for the light based on time passed
        //     echolocationLight.intensity = Mathf.Lerp(1, 0, elapsedTime / echolocationDuration);

        //     // Calculate the color from white to black based on time passed
        //     Color newColor = Color.Lerp(Color.white, Color.black, elapsedTime / echolocationDuration);
        //     echolocationTilemapRenderer.color = newColor;

        //     // Increase the elapsed time
        //     elapsedTime += Time.deltaTime;

        //     // Wait for the next frame
        //     yield return null;
        // }
        // // Ensure the light intensity and tilemap color are set to final values at the end
        // echolocationLight.intensity = 0;
        // echolocationTilemapRenderer.color = Color.black;
        canEcholocate = true;
    }

    void FixedUpdate()
    {
        // Handle horizontal movement
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

        GameObject echolocationLightObject = echolocationLight.gameObject;
        Transform blackCenter = echolocationLightObject.transform.GetChild(0);
        blackCenter.gameObject.SetActive(false);



        // echolocationTilemapRenderer.color = Color.white;

    }
}