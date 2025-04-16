using UnityEngine;
using System.Collections;

public class CentralEye : MonoBehaviour
{
    public float heightAbovePlayer = 5f;
    public float moveSpeed = 3f;
    public float targetingDelay = 0.5f;
    public float followSmoothTime = 0.3f;
    public LineRenderer lineRenderer;
    public float laserDamage = 30f;
    public float laserDuration = 1f;
    public Transform BoltPoint1;
    public Transform BoltPoint2;
    public GameObject boltPrefab;
    public Transform firePoint;
    
    private Transform target;
    private Vector3 currentVelocity;
    private Vector3 lastKnownPlayerPos;
    private EdgeCollider2D edgeCollider;
    private Vector3 originalPosition;
    private bool movingForLaser = false;
    private bool isAttackInProgress = false;
    private bool isLaserActive = false;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        originalPosition = transform.position;
        edgeCollider = GetComponent<EdgeCollider2D>();
        
        // Ignore collision with terrain
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Default"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Climbable"), true);
    }

    void Update()
    {
        if (target == null) return;

        if (movingForLaser || isLaserActive)
        {
            // Move above player
            Vector3 targetPosition = target.position + Vector3.up * heightAbovePlayer;
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                targetPosition, 
                ref currentVelocity, 
                followSmoothTime
            );

            // Disable collider when moving into position
            if (edgeCollider.enabled && Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                edgeCollider.enabled = false;
            }
        }
    }

    public bool IsAttacking()
    {
        return isAttackInProgress;
    }

    public void LaserAttack()
    {
        if (!isAttackInProgress)
        {
            StartCoroutine(LaserAttackSequence());
        }
    }

    public void BoltAttack(int numberOfProjectiles)
    {
        if (!isAttackInProgress)
        {
            StartCoroutine(BoltAttackSequence(numberOfProjectiles));
        }
    }

    private IEnumerator LaserAttackSequence()
    {
        isAttackInProgress = true;
        movingForLaser = true;
        
        // Move into position and wait
        yield return new WaitForSeconds(1f);
        
        // Fire laser and follow player
        isLaserActive = true;
        movingForLaser = false;
        StartCoroutine(LaserSequence());
        yield return new WaitForSeconds(laserDuration);
        isLaserActive = false;

        // Return to original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                originalPosition,
                ref currentVelocity,
                followSmoothTime
            );
            yield return null;
        }

        edgeCollider.enabled = true;
        isAttackInProgress = false;
    }

    private IEnumerator BoltAttackSequence(int numberOfProjectiles)
    {
        isAttackInProgress = true;
        edgeCollider.enabled = false;

        // Move to first point
        while (Vector3.Distance(transform.position, BoltPoint1.position) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                BoltPoint1.position,
                ref currentVelocity,
                followSmoothTime
            );
            yield return null;
        }

        // Move to second point while spawning projectiles
        float totalDistance = Vector3.Distance(BoltPoint1.position, BoltPoint2.position);
        float distancePerProjectile = totalDistance / (numberOfProjectiles - 1);
        Vector3 direction = (BoltPoint2.position - BoltPoint1.position).normalized;
        float journeyLength = 0;

        while (Vector3.Distance(transform.position, BoltPoint2.position) > 0.1f)
        {
            Vector3 lastPosition = transform.position;
            transform.position = Vector3.SmoothDamp(
                transform.position,
                BoltPoint2.position,
                ref currentVelocity,
                followSmoothTime
            );

            // Check if we've moved far enough to spawn next projectile
            journeyLength += Vector3.Distance(lastPosition, transform.position);
            if (journeyLength >= distancePerProjectile)
            {
                Instantiate(boltPrefab, transform.position, Quaternion.identity);
                journeyLength = 0;
            }

            yield return null;
        }

        // Return to original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                originalPosition,
                ref currentVelocity,
                followSmoothTime
            );
            yield return null;
        }

        edgeCollider.enabled = true;
        isAttackInProgress = false;
    }

    private IEnumerator LaserSequence()
    {
        float elapsed = 0f;
        int layerMask = ~LayerMask.GetMask("Enemy");
        
        while (elapsed < laserDuration)
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, Vector2.down, heightAbovePlayer * 2f, layerMask);
            
            Vector2 laserEnd = hit.collider != null ? hit.point : (Vector2)firePoint.position + Vector2.down * heightAbovePlayer * 2f;
            
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerHealth>()?.Damage(laserDamage * Time.deltaTime);
            }

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, laserEnd);

            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeLaser());
    }

    IEnumerator FadeLaser()
    {
        Material lineMaterial = lineRenderer.material;
        Color initialColor = lineMaterial.GetColor("_EmissionColor");
        
        yield return new WaitForSeconds(laserDuration * 0.5f);

        float elapsed = 0f;
        float fadeDuration = laserDuration * 0.5f;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float fadeAmount = 1f - (elapsed / fadeDuration);
            lineMaterial.SetColor("_EmissionColor", initialColor * fadeAmount);
            yield return null;
        }

        lineRenderer.enabled = false;
        lineMaterial.SetColor("_EmissionColor", initialColor);
    }
}
