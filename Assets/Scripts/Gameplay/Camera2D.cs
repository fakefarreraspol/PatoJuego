using UnityEngine;
using System.Collections;

public class Camera2D : MonoBehaviour
{

    public Transform target;  // The target object (the sprite in this case)
    public float smoothTime = 0.3f;  // Smoothing time for the camera movement
    public Vector3 offset;  // Offset between the camera and the target

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the target position with offset
            Vector3 targetPosition = target.position + offset;

            // Smoothly interpolate towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else if (FindObjectOfType<Character>() != null) 
        {
            target = FindObjectOfType<Character>().transform;
        }
    }
}
