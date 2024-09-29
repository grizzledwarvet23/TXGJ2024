using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8;
    public Rigidbody2D rb;
    LevelMetadata metadata;
    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        metadata = FindObjectOfType<LevelMetadata>();
        if(metadata.isPlayerAActive()) { // shooter active, pause and store bullets
            Debug.Log("Player A is active");
        } else { // shield active, run bullets
            Debug.Log("setting velocity");
            Debug.Log(direction);
            rb.velocity = direction * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo) {
        Debug.Log(hitInfo);
        if(hitInfo.gameObject.tag == "Enemy") {
            Destroy(hitInfo.gameObject);
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        if (!metadata.isPlayerAActive() && direction != null) {
            Debug.Log("setting velocity");
            Debug.Log(direction * speed);
            rb.velocity = direction * speed;
        }
    }

    public void setDirection(Vector2 dir) {
        Debug.Log("setting direction");
        Debug.Log(direction);
        direction = dir;
    }
}
