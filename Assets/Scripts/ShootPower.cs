using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPower : MonoBehaviour
{
    private Transform player; // Reference to the player object
    public Transform firePoint; // The fire point that will rotate and move
    public GameObject bulletPrefab;
    public GameObject bulletsParent; // Parent object for bullets
    public float rotationSpeed = 5f; // Speed of rotation
    public float firePointDistance = 1.5f; // Distance from the player to the fire point

    void Start()
    {
        player = gameObject.transform;
    }
    void Update()
    {
        AimTowardsMouse();
        
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void AimTowardsMouse()
    {
        // Get the mouse position in the world
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Keep the z-coordinate at 0 for 2D gameplay

        // Calculate the direction from the player to the mouse position
        Vector2 direction = (mousePosition - player.position).normalized;

        // Calculate the angle in degrees to rotate
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Update fire point position based on angle and distance
        firePoint.position = player.position + (Vector3)(direction * firePointDistance);

        // Smoothly rotate the fire point towards the angle
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        firePoint.rotation = targetRotation;
    }

    void Shoot()
    {
        // Shooting logic: instantiate the bullet under the bulletsParent
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, bulletsParent.transform);

        // Set the bullet's direction
        Vector3 bulletDirection = (firePoint.up).normalized; // Assuming the bullet moves in the up direction of the fire point
        Bullet bullet = newBullet.GetComponent<Bullet>();
        if (bullet != null)
        {
            // bullet.SetDirection(bulletDirection); // Assuming you have a SetDirection method in your Bullet class
        }
    }
}