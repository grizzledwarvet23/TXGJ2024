using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject != null && other.gameObject.tag == "Player")
        {
            
            // other.GetComponent<Player>().TakeDamage(1);
            other.GetComponent<Player>().setPlayerBeingPushed(true);
            Vector2 pushDirection = other.transform.position - transform.position;
            other.GetComponent<Rigidbody2D>().AddForce(pushDirection.normalized * 10, ForceMode2D.Impulse);
            
        }
        
    }
}
