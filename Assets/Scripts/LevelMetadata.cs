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
        yield return new WaitForSeconds(0.5f);
        if(playerAisActive)
        {
            playerB.SetActive(true);
            playerA.SetActive(false);
        } else {
            playerA.SetActive(true);
            playerB.SetActive(false);
        }
        playerAisActive = !playerAisActive;
        isSwitching = false;



    }
}
