using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    public LayerMask WallLayer;
    public Transform firePoint;
    public Transform mountPoint;
    public float chargeTime = 5f;
    public LineRenderer lineRenderer;
    public Transform target;
    private Vector3 targetPos {get => target.position + Vector3.up;}
    public EnemyHealth selfHealth;
    public float projectileSpeed = 10f;
    public float rotationSpeed = 5f;
    public SpriteRenderer mountSprite;
    private float currentChargeTime = 0f;
    private bool isCharging = false;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        WallLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable");
        selfHealth = GetComponent<EnemyHealth>();
        originalColor = mountSprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 directionToTarget = (targetPos - firePoint.position).normalized;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        
        // Smoothly rotate towards target
            mountPoint.rotation = Quaternion.Slerp(
            mountPoint.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );

        // Check line of sight
        if (!Physics2D.Raycast(firePoint.position, directionToTarget, Vector3.Distance(firePoint.position, targetPos), WallLayer)) 
        {
            if (!isCharging)
            {
                isCharging = true;
                currentChargeTime = 0f;
                Debug.Log("Charging");
            }

            if (isCharging)
            {
                currentChargeTime += Time.deltaTime;
                
                // Lerp color from original to red
                float chargeProgress = currentChargeTime / chargeTime;
                mountSprite.color = Color.Lerp(originalColor, Color.red, chargeProgress);

                // Fire when fully charged
                if (currentChargeTime >= chargeTime)
                {
                    ShootBeam();
                    ResetCharge();
                }
            }
        }
    }

    private void ResetCharge()
    {
        isCharging = false;
        currentChargeTime = 0f;
        mountSprite.color = originalColor;
    }

    private void Draw2DRay(Vector2 start, Vector2 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private IEnumerator DisableLineAfterTime(float duration)
    {
        float elapsedTime = 0f;
        Material lineMaterial = lineRenderer.material; 
        Color initialColor = lineMaterial.GetColor("_EmissionColor"); 
        float initialIntensity = initialColor.maxColorComponent;  

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fadeFactor = Mathf.Lerp(initialIntensity, 0f, elapsedTime / duration);  
            Color newColor = initialColor * fadeFactor; 
            lineMaterial.SetColor("_EmissionColor", newColor);  
            yield return null;  
        }

        lineMaterial.SetColor("_EmissionColor", Color.black);  
        lineRenderer.enabled = false; 
        lineRenderer.material.SetColor("_EmissionColor", initialColor);
    }
    private void ShootBeam()
    {
        int layerMask = ~LayerMask.GetMask("Enemy");

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, mountPoint.right, 100f, layerMask);

        if (hit.collider != null)
        {
            Debug.Log("Hit");
            Draw2DRay(firePoint.position, hit.point);
            // Check what layer the hit object is on
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Hit an player!");
                hit.collider.gameObject.GetComponent<PlayerHealth>().Damage(30);
            }
            else
            {
                Debug.Log("Hit something else.");
            }
        } else {
            Draw2DRay(firePoint.position, firePoint.position + mountPoint.right * 100f);
        }

        StartCoroutine(DisableLineAfterTime(0.5f));
    }
}
