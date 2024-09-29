using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootPower : MonoBehaviour
{
    
    public Transform firePoint;
    public GameObject bulletPrefab;
    Bullet bullet;
    // Update is called once per frame

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    void Shoot() {
        // shooting logic
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // get mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // calculate direction from player to the mouse position
        Vector2 direction = (mousePosition - transform.position).normalized;
        direction.Normalize();

        // bullet = bulletPrefab.GetComponent<Bullet>();
        // bullet.setDirection(direction);
    }
}
