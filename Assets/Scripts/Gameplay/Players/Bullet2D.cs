using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2D : MonoBehaviour
{

    public float speed = 10f;
   
    public Vector2 dir = Vector2.zero;
    void Update()
    {
        // Move the bullet forward
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet collided with an object on the "Ground" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Destroy the bullet when it hits the ground
            Destroy(gameObject);
        }
    }
}
