using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour, Enemy
{

    public Transform groundCheck;
    public LayerMask groundMask;

    private Rigidbody2D rb;

    private bool movingRight = true;

    private int health = 2;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        float moveSpeed = movingRight ? 2 : -2;
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    public void TakeDamage(int d)
    {
        health -= d;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        // get component in children Pusher.cs:
        Pusher pusher = GetComponentInChildren<Pusher>();
        pusher.movingRight = !pusher.movingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;   
    }

    // Update is called once per frame
    void Update()
    {
        // create a raycast from groundcheck that shoots downwards:
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundMask);
        if(hit.collider == null)
        {
            Flip();
        }
    }

}
