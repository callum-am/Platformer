using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public List<Eye> eyes = new List<Eye>();
    public float rotationSpeed = 30f;
    public float chaseSpeed = 2f;
    public float catchRadius = 1f;
    private Transform target;
    private bool isTrackingPlayer = false;
    private bool isChasing = false;
    private PlayerController.PlayerController playerController;
    private SunSprite targetSunSprite;
    private bool firingCosmeticLasers = false;
    private bool increasingLaserIntensity = false;
    public float maxLaserIntensity = 5f;
    public float intensityIncreaseRate = 0.5f;
    private bool isExecutingFinalSequence = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (target != null)
        {
            playerController = target.GetComponent<PlayerController.PlayerController>();
        }
        targetSunSprite = FindObjectOfType<SunSprite>();
    }

    void Update()
    {
        if (isTrackingPlayer)
        {
            TrackPlayerPosition();
        }

        if (isChasing && target != null)
        {
            ChaseAndCatchPlayer();
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

    private void ChaseAndCatchPlayer()
    {
        if (Vector2.Distance(transform.position, target.position) <= catchRadius)
        {
            CatchPlayer();
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)(direction * chaseSpeed * Time.deltaTime);
    }

    private void CatchPlayer()
    {
        
        isChasing = false;
        isExecutingFinalSequence = true;
        if (playerController != null)
        {
            playerController.TogglePlayer(false);
            Vector3 centerPosition = transform.position;
            target.position = centerPosition; // Center player on wheel
            StartCoroutine(MaintainPlayerPosition(centerPosition));
        }
        StartCosmeticLasers();
    }

    public bool IsFinalSequenceActive()
    {
        return isExecutingFinalSequence;
    }

    private IEnumerator MaintainPlayerPosition(Vector3 position)
    {
        while (!isChasing)
        {
            if (target != null)
            {
                target.position = position;
            }
            yield return null;
        }
    }

    private void StartCosmeticLasers()
    {
        StartPlayerTracking();
        if (targetSunSprite == null) return;
        firingCosmeticLasers = true;
        StartCoroutine(CosmeticLaserRoutine());
        targetSunSprite.BeginApproachingPlayer();
    }

    public void StartLaserIntensityIncrease()
    {
        increasingLaserIntensity = true;
    }

    private IEnumerator CosmeticLaserRoutine()
    {
        float currentIntensity = 1f;
        bool hasDeltFinalDamage = false;
        
        while (firingCosmeticLasers)
        {
            for (int i = 0; i < eyes.Count; i++)
            {
                if (i < targetSunSprite.wheelEyePositions.Count)
                {
                    Vector3 targetPos = targetSunSprite.transform.TransformPoint(targetSunSprite.wheelEyePositions[i]);
                    eyes[i].FireCosmeticLaser(targetPos, currentIntensity);
                }
            }

            if (increasingLaserIntensity)
            {
                currentIntensity = Mathf.Min(currentIntensity + intensityIncreaseRate * Time.deltaTime, maxLaserIntensity);
                
                // When max intensity is reached, deal final damage
                if (currentIntensity >= maxLaserIntensity && !hasDeltFinalDamage)
                {
                    if (target != null)
                    {
                        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                        if (playerHealth != null)
                        {
                            playerHealth.Damage(playerHealth.health);
                            hasDeltFinalDamage = true;
                        }
                    }
                }
            }

            yield return null;
        }
    }

    public void StopCosmeticLasers()
    {
        firingCosmeticLasers = false;
        foreach (var eye in eyes)
        {
            eye.StopLaser();
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

    public void StartChasing()
    {
        isChasing = true;
    }

    public void StopChasing()
    {
        isChasing = false;
        StopCosmeticLasers();
        if (playerController != null)
        {
            playerController.TogglePlayer(true);
        }
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
