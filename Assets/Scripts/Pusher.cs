using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{

    [System.NonSerialized]
    public bool movingRight = true;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject != null && other.gameObject.tag == "Player")
        {
            
            // other.GetComponent<Player>().TakeDamage(1);
            other.GetComponent<Player>().setPlayerBeingPushed(true, 1.2f);
            // Vector2 pushDirection = other.transform.position - transform.position;
            // just make push direction based on moving right:
            Vector2 pushDirection = movingRight ? new Vector2(1, 1) : new Vector2(-1, 1);
            pushDirection = new Vector2(pushDirection.x * 1.5f, pushDirection.y * 2);
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            other.GetComponent<Rigidbody2D>().AddForce(pushDirection.normalized * 25, ForceMode2D.Impulse);
            
        }
        
    }
}
