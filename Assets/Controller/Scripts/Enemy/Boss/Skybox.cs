using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    [Header("Parallax Settings")]
    public float parallaxStrength = 0.1f;
    public float maxMovementRange = 1f;
    
    private Camera mainCamera;
    private Vector3 centerPosition;

    void Start()
    {
        mainCamera = Camera.main;
        centerPosition = transform.position;
    }

    void Update()
    {
        // Calculate offset from camera's current position relative to skybox center
        Vector2 cameraOffset = mainCamera.transform.position - centerPosition;
        
        // Calculate inverse movement (parallax effect)
        Vector2 parallaxOffset = -cameraOffset * parallaxStrength;
        
        // Clamp the movement within the maximum range from center
        parallaxOffset = Vector2.ClampMagnitude(parallaxOffset, maxMovementRange);
        
        // Apply the movement relative to center position
        transform.position = centerPosition + (Vector3)parallaxOffset;
    }
}
