using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class LevelMetadata : MonoBehaviour
{
    public GameObject playerA;
    public GameObject playerB;

    private bool playerAisActive = true;

    public TextMeshProUGUI timerText;

    public Color timerColorA;
    public Color timerColorB;

    public float timerLength = 5.0f;

    private float currentTimer;

    private bool isSwitching = false;

    public AudioSource musicTrackA;
    public AudioSource musicTrackB;

    private float trackAPosition = 0.0f;
    private float trackBPosition = 0.0f;

    public AudioSource wakeupSound;

    public GameObject playerA_UI;
    public GameObject playerB_UI;

    public GameObject vcamA;
    public GameObject vcamB;

    public bool setPlayersOff = false;

    public Slider timerSlider;
    public Image knobImage;

    // Start is called before the first frame update
    void Start()
    {
        currentTimer = playerAisActive ? 0 : timerLength;

        if(vcamA == null)
        {
            Debug.Log("vcam A is null! (LevelMetadata)");
        }
        if(vcamB == null)
        {
            Debug.Log("vcam B is null! (LevelMetadata)");
        }
        
        // Set the initial timer color based on the active player
        timerText.color = playerAisActive ? timerColorA : timerColorB;
        Debug.Log(timerColorA);
        Debug.Log(timerText.color);

        if(timerSlider != null)
        {
            timerSlider.maxValue = 10;
            timerSlider.value = currentTimer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }

        if(
            // playerAisActive && 
        Input.GetKeyDown(KeyCode.LeftShift))
        {
            Switch();
        }

        if (!isSwitching)
        {
            if(playerAisActive) //counting up
            {
                currentTimer += Time.deltaTime;
            } else {
                currentTimer -= Time.deltaTime;
            }

            if(timerSlider != null)
            {
                timerSlider.value = currentTimer;
            }

            if (playerAisActive && currentTimer >= 10 || !playerAisActive && currentTimer <= 0)
            {
                Switch();
            }

            int seconds = Mathf.FloorToInt(Mathf.Max(0, currentTimer));  // Get the whole seconds
            int milliseconds = Mathf.Max(0, Mathf.FloorToInt((currentTimer - seconds) * 100));  // Get the milliseconds

            // Update the timerText UI element
            timerText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
            
        }
    }

    void Switch()
    {
        // basically the timer goes from counting up to counting down.
        // currentTimer = 10 - currentTimer;
        StartCoroutine(SwitchCharacters());

        if (knobImage != null)
        {
            knobImage.color = playerAisActive ? timerColorB : timerColorA;
        } else {
            Debug.Log("KNOB NOT ASSIGNED!");
        }
    }

    IEnumerator SwitchCharacters()
    {
        isSwitching = true;

        if (wakeupSound != null) {
            wakeupSound.Play();
        }

        // Pause music and track positions for the currently active player
        if (playerAisActive)
        {
            trackAPosition = musicTrackA != null ? musicTrackA.time : 0;
            if (musicTrackA != null) {
                musicTrackA.Stop();
            }
            if (musicTrackB != null) {
                musicTrackB.time = trackBPosition;
                musicTrackB.Play();
            }
        }
        else
        {
            trackBPosition = musicTrackB != null ? musicTrackB.time : 0;
            if (musicTrackB != null) {
                musicTrackB.Stop();
            }
            if (musicTrackA != null) {
                musicTrackA.time = trackAPosition;
                musicTrackA.Play();
            }
        }

        // Suspension delay
        yield return new WaitForSeconds(0.1f);

        // Switch character active states and update UI elements
        if (playerAisActive)
        {
            

            

            if (playerB_UI != null) {
                playerB_UI.SetActive(true);
            }
            if (playerA_UI != null) {
                playerA_UI.SetActive(false);
            }

            // Change timer text color for player B
            timerText.color = timerColorB;

            if(!setPlayersOff) {
                DisablePlayer(playerA);
                EnablePlayer(playerB);
            } else {
                playerA.SetActive(false);
                playerB.SetActive(true);
            }

            // playerA.SetActive(false);
            // playerB.SetActive(true);
            playerB.GetComponent<Player>().OnSwitch();

            
            
        }
        else
        {

            

            if (playerA_UI != null) {
                playerA_UI.SetActive(true);
            }
            if (playerB_UI != null) {
                playerB_UI.SetActive(false);
            }

            // Change timer text color for player A
            timerText.color = timerColorA;

            if(!setPlayersOff) {
                DisablePlayer(playerB);
                EnablePlayer(playerA);
            } else {
                playerB.SetActive(false);
                playerA.SetActive(true);
            }

            // playerB.SetActive(false);
            // playerA.SetActive(true);
            playerA.GetComponent<Player>().OnSwitch();


            
        }

        // Toggle active player status and exit switching state
        playerAisActive = !playerAisActive;
        isSwitching = false;
    }

    void EnablePlayer(GameObject player)
    {
        if(!player.activeSelf)
        {
            player.SetActive(true);
        }
        // Enable all components on the player
        foreach (var component in player.GetComponents<Component>())
        {
            if (component is Behaviour behaviour)
                behaviour.enabled = true;
        }

        foreach (Transform child in player.transform)
        {
            child.gameObject.SetActive(true);
        }

        // Set the SpriteRenderer's opacity to 1
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 1.0f;
            spriteRenderer.color = color;
        }
        
    }

    void DisablePlayer(GameObject player)
    {
        // Disable all components except SpriteRenderer
        foreach (var component in player.GetComponents<Component>())
        {
            if (component is Behaviour behaviour && !(component is SpriteRenderer))
                behaviour.enabled = false;
        }

        foreach (Transform child in player.transform)
        {
            child.gameObject.SetActive(false);
        }

        // Set the SpriteRenderer's opacity to 0.5
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 0.25f;
            spriteRenderer.color = color;
        }

       
    }


    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Replace "MainMenu" with the actual name of your main menu scene
    }

    public bool isPlayerAActive() {
        return playerAisActive;
    }
}