using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2D : MonoBehaviour
{

    public float speed = 10f;
    public float lifetime = 2.0f; // Set the lifetime in seconds
    public Vector2 dir = Vector2.zero;
    void Update()
    {
        // Move the bullet forward
        transform.Translate(dir * speed * Time.deltaTime);

        // Decrease the lifetime
        lifetime -= Time.deltaTime;

        // Check if the lifetime has expired
        if (lifetime <= 0.0f)
        {
            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
