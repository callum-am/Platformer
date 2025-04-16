using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    private Quaternion localDefaultRotation; // Change from defaultRotation to localDefaultRotation
    private Transform target;
    private bool isTracking = false;
    public float rotationSpeed = 5f;
    public ProjectileSpawner projectileSpawner;
    public enum FiringMode { Straight, Spin }
    private bool isFiring = false;
    public LineRenderer lineRenderer;
    public float laserDuration = 5f;
    public float laserDamage = 30f;
    public Transform firePoint;
    private bool canDamagePlayer = true;
    private Vector2 currentLaserEnd;
    private bool isLaserActive = false;
    private Material lineMaterial;
    private Color initialLaserColor;

    void Start()
    {
        localDefaultRotation = transform.localRotation; // Store local rotation instead of world rotation
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (projectileSpawner == null)
        {
            projectileSpawner = GetComponent<ProjectileSpawner>();
        }

        lineMaterial = lineRenderer.material;
        initialLaserColor = lineMaterial.GetColor("_EmissionColor");
    }

    void Update()
    {
        if (target == null) return;

        if (isTracking)
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            
            // Use world rotation when tracking player
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            // Use local rotation when not tracking to maintain position relative to wheel
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                localDefaultRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void StartTracking()
    {
        isTracking = true;
    }

    public void StopTracking()
    {
        isTracking = false;
    }

    public void StartFiring(FiringMode mode)
    {
        if (projectileSpawner != null)
        {
            isFiring = true;
            switch (mode)
            {
                case FiringMode.Straight:
                    projectileSpawner.SetStraightFiring();
                    break;
                case FiringMode.Spin:
                    projectileSpawner.SetSpinFiring();
                    break;
            }
            projectileSpawner.StartFiring();
        }
    }

    public void StopFiring()
    {
        if (projectileSpawner != null)
        {
            isFiring = false;
            projectileSpawner.StopFiring();
        }
    }

    public bool IsFiring()
    {
        return isFiring;
    }

    public void SetSpawnRate(float rate)
    {
        if (projectileSpawner != null)
        {
            projectileSpawner.SetSpawnRate(rate);
        }
    }

    public void FireLaser()
    {
        if (isLaserActive) return; // Prevent multiple concurrent lasers
        
        isLaserActive = true;
        lineRenderer.enabled = true;
        lineMaterial.SetColor("_EmissionColor", initialLaserColor);
        StartCoroutine(LaserActiveCheck());
        StartCoroutine(PersistentLaserEffect(laserDuration));
    }

    private IEnumerator LaserActiveCheck()
    {
        while (isLaserActive)
        {
            CheckLaserCollision();
            yield return new WaitForFixedUpdate(); // Use FixedUpdate for more stable physics checks
        }
    }

    private void CheckLaserCollision()
    {
        int layerMask = ~LayerMask.GetMask("Enemy");
        Vector2 firingDirection = Quaternion.Euler(0, 0, 90) * transform.right;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firingDirection, 100f, layerMask);

        if (hit.collider != null)
        {
            currentLaserEnd = hit.point;
            
            // Handle player damage
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player") && canDamagePlayer)
            {
                hit.collider.gameObject.GetComponent<PlayerHealth>()?.Damage(laserDamage);
                StartCoroutine(DamageCooldown());
            }

            // Notify platform of hit
            BossPlatform platform = hit.collider.GetComponent<BossPlatform>();
            if (platform != null)
            {
                platform.OnLaserHit(true);
            }
        }
        else
        {
            currentLaserEnd = firePoint.position + (Vector3)(firingDirection * 100f);
        }

        Draw2DRay(firePoint.position, currentLaserEnd);
    }

    private IEnumerator DamageCooldown()
    {
        canDamagePlayer = false;
        yield return new WaitForSeconds(1f);
        canDamagePlayer = true;
    }

    private void Draw2DRay(Vector2 start, Vector2 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private IEnumerator PersistentLaserEffect(float duration)
    {
        // Hold phase
        yield return new WaitForSeconds(duration * 0.8f); // Hold for 80% of duration

        // Fade phase
        float fadeDuration = duration * 0.2f; // Fade for last 20% of duration
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fadeFactor = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            lineMaterial.SetColor("_EmissionColor", initialLaserColor * fadeFactor);
            yield return null;
        }

        isLaserActive = false;
        lineRenderer.enabled = false;
        lineMaterial.SetColor("_EmissionColor", initialLaserColor);
    }
}
