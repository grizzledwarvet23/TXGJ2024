using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Bullet : MonoBehaviour
{

    public float slowSpeed = 0.1f;
    public float speed = 8f; // Speed of the bullet
    public Rigidbody2D rb; // Rigidbody2D component
    private Vector2 direction; // Direction the bullet will move
    public bool active = false; // Controls whether the bullet moves

    public AudioSource slowDownSound;
    public AudioSource speedUpSound;

    void Start()
    {
        if(!active)
        {
            slowDownSound.Play();
        }
    }

    void Update()
    {
        // Update bullet velocity based on active status
        if (active)
        {
            rb.velocity = transform.right * speed; // Move the bullet
        }
        else
        {
            rb.velocity = transform.right * slowSpeed;
        }
    }

    public void SetActive(bool boo)
    {
        active = boo;
        if(!active)
        {
            slowDownSound.Play();
        } else if(active)
        {
            speedUpSound.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the bullet
        }

        else if(hitInfo.CompareTag("DoorSwitch"))
        {
            hitInfo.gameObject.GetComponent<DoorSwitch>().ActivateDoor();
        }

        else if(hitInfo.CompareTag("FinalBoss"))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

}