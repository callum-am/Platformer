using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public List<Eye> eyes = new List<Eye>();
    public float rotationSpeed = 30f;
    private Transform target;
    private bool isTrackingPlayer = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isTrackingPlayer)
        {
            TrackPlayerPosition();
        }
    }

    private void TrackPlayerPosition()
    {
        if (target == null) return;
        
        // Calculate rotation direction
        float rotationAmount = rotationSpeed * Time.deltaTime;
        if (transform.position.x > target.position.x)
        {
            // Player is on the left, rotate counter-clockwise
            transform.Rotate(0, 0, rotationAmount);
        }
        else
        {
            // Player is on the right, rotate clockwise
            transform.Rotate(0, 0, -rotationAmount);
        }
    }

    public void StartPlayerTracking()
    {
        isTrackingPlayer = true;
    }

    public void StopPlayerTracking()
    {
        isTrackingPlayer = false;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    // Methods to control individual eyes
    public void SetEyeTracking(int eyeIndex, bool tracking)
    {
        if (IsValidEyeIndex(eyeIndex))
        {
            if (tracking)
                eyes[eyeIndex].StartTracking();
            else
                eyes[eyeIndex].StopTracking();
        }
    }

    public void SetEyeFiring(int eyeIndex, Eye.FiringMode mode)
    {
        if (IsValidEyeIndex(eyeIndex))
        {
            eyes[eyeIndex].StartFiring(mode);
        }
    }

    public void StopEyeFiring(int eyeIndex)
    {
        if (IsValidEyeIndex(eyeIndex))
        {
            eyes[eyeIndex].StopFiring();
        }
    }

    public void FireEyeLaser(int eyeIndex)
    {
        if (IsValidEyeIndex(eyeIndex))
        {
            eyes[eyeIndex].FireLaser();
        }
    }

    // Group control methods
    public void SetAllEyesTracking(bool tracking)
    {
        foreach (var eye in eyes)
        {
            if (tracking)
                eye.StartTracking();
            else
                eye.StopTracking();
        }
    }

    public void SetAllEyesFiring(Eye.FiringMode mode)
    {
        foreach (var eye in eyes)
        {
            eye.StartFiring(mode);
        }
    }

    public void StopAllEyesFiring()
    {
        foreach (var eye in eyes)
        {
            eye.StopFiring();
        }
    }

    public void FireAllEyeLasers()
    {
        foreach (var eye in eyes)
        {
            eye.FireLaser();
        }
    }

    public void SetAllEyesSpawnRate(float spawnRate)
    {
        foreach (var eye in eyes)
        {
            eye.SetSpawnRate(spawnRate);
        }
    }

    private bool IsValidEyeIndex(int index)
    {
        return index >= 0 && index < eyes.Count;
    }
}
