using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelMetadata : MonoBehaviour
{

    public Player playerA;
    public Player playerB;

    public TextMeshProUGUI timerText;

    public float timerLength = 10.0f;
    private bool isSwitching = false;

    // Start is called before the first frame update
    void Start()
    {
        // lets start 
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSwitching) {
            timerLength -= Time.deltaTime;

            if(timerLength <= 0)
            {
                timerLength = 10.0f;
                StartCoroutine(SwitchCharacters());
                
            }

            int seconds = Mathf.FloorToInt(timerLength);  // Get the whole seconds
            int milliseconds = Mathf.FloorToInt((timerLength - seconds) * 100);  // Get the milliseconds

            // Update the timerText UI element
            timerText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
        }

    }

    IEnumerator SwitchCharacters()
    {
        isSwitching = true;
        yield return new WaitForSeconds(0.5f);


    }
}
