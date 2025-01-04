using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Enum to specify movement direction
    public enum Direction { Horizontal, Vertical }

    [Header("Movement Settings")]
    public Direction movementDirection = Direction.Horizontal; // Choose horizontal or vertical
    public float speed = 2f; // Speed of the platform
    public float distance = 5f; // Distance the platform moves

    private Vector3 startPos; // Starting position of the platform
    private int direction = 1; // Movement direction multiplier (1 or -1)

    private Rigidbody2D rb;

    void Start()
    {
        // Save the starting position of the platform
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Calculate the movement vector
        Vector2 movement;
        if (movementDirection == Direction.Horizontal)
        {
            rb.velocity = Vector2.right * direction * speed;
            if(direction == 1 && transform.position.x >= startPos.x + distance)
            {
                direction = -1;
            }
            else if(direction == -1 && transform.position.x <= startPos.x)
            {
                direction = 1;
            }
        }
        else if (movementDirection == Direction.Vertical)
        {
            rb.velocity = Vector2.up * direction * speed;
            if(direction == 1 && transform.position.y >= startPos.y + distance)
            {
                direction = -1;
            }
            else if(direction == -1 && transform.position.y <= startPos.y)
            {
                direction = 1;
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (movementDirection == Direction.Horizontal)
        {
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(transform.position.x + distance, transform.position.y, 0));
        }
        else if (movementDirection == Direction.Vertical)
        {
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(transform.position.x, transform.position.y + distance, 0));
        }
    }


}