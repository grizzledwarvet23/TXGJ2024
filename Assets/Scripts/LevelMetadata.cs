using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelMetadata : MonoBehaviour
{

    public GameObject playerA;
    public GameObject playerB;

    private bool playerAisActive = true;

    public TextMeshProUGUI timerText;

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
    

    // Start is called before the first frame update
    void Start()
    {
        // lets start 
        currentTimer = timerLength;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSwitching) {
            currentTimer -= Time.deltaTime;

            if(currentTimer <= 0)
            {
                currentTimer = timerLength;
                StartCoroutine(SwitchCharacters());
                
            }

            int seconds = Mathf.FloorToInt(currentTimer);  // Get the whole seconds
            int milliseconds = Mathf.FloorToInt((currentTimer - seconds) * 100);  // Get the milliseconds

            // Update the timerText UI element
            timerText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
        }

    }

    IEnumerator SwitchCharacters()
    {
        isSwitching = true;
        if(wakeupSound != null) {
            wakeupSound.Play();
        }
        if(playerAisActive)
        {
            trackAPosition = musicTrackA != null ? musicTrackA.time : 0;
            if(musicTrackA != null) {
                musicTrackA.Stop();
            }
            if(musicTrackB != null) {
                musicTrackB.Play();
            }
        }
        else {
            trackBPosition = musicTrackB != null ? musicTrackB.time : 0;
            if(musicTrackB != null) {
                musicTrackB.Stop();
            }
            if(musicTrackA != null) {
                musicTrackA.Play();
            }
        }


        yield return new WaitForSeconds(0.1f);
        if(playerAisActive)
        {
            playerB.SetActive(true);
            playerA.SetActive(false);

            
            playerB.GetComponent<Player>().OnSwitch();
            

            if(playerB_UI != null) {
                playerB_UI.SetActive(true);
            }
            if(playerA_UI != null) {
                playerA_UI.SetActive(false);
            }
        } else {
            playerA.SetActive(true);
            playerB.SetActive(false);

            
            playerA.GetComponent<Player>().OnSwitch();
            
            if(playerA_UI != null) {
                playerA_UI.SetActive(true);
            }
            if(playerB_UI != null) {
                playerB_UI.SetActive(false);
            }
        }
        playerAisActive = !playerAisActive;
        isSwitching = false;



    }

    public bool isPlayerAActive() {
        return playerAisActive;
    }
}
