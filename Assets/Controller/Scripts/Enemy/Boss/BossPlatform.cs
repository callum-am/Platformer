using UnityEngine;
using System.Collections;

public class BossPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    public float maxDurability = 100f;
    public float currentDurability;
    public float regenerationRate = 5f; // Per second
    public float projectileDamage = 10f;
    public float singleLaserDamage = 0.1f;  // New property for single-hit damage
    private BoxCollider2D platformCollider;
    private SpriteRenderer spriteRenderer;
    private Material lineMaterial;
    private Color initialEmissionColor;
    private bool canRegenerate = true;
    private bool isRegenerating = false;
    private WaitForSeconds regenerationDelay = new WaitForSeconds(2f);

    [Header("Visual Settings")]
    public Color emissionColor = Color.cyan;
    public float maxEmissionIntensity = 3f;

    void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Setup emission properly
        lineMaterial = new Material(spriteRenderer.sharedMaterial);
        spriteRenderer.material = lineMaterial;  // Assign the instance to avoid sharing
        
        currentDurability = maxDurability;
        
        lineMaterial.EnableKeyword("_EMISSION");
        initialEmissionColor = emissionColor * maxEmissionIntensity;
        lineMaterial.SetColor("_EmissionColor", initialEmissionColor);

        // Set initial transparency
        Color baseColor = lineMaterial.color;
        baseColor.a = 0.5f;
        lineMaterial.color = baseColor;
    }

    void Update()
    {
        if (isRegenerating && canRegenerate && currentDurability < maxDurability)
        {
            currentDurability = Mathf.Min(currentDurability + regenerationRate * Time.deltaTime, maxDurability);
            UpdateEmission();

            if (currentDurability > 0 && !platformCollider.enabled)
            {
                platformCollider.enabled = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            TakeDamage(projectileDamage);
            Destroy(collision.gameObject);
        }
    }
    public void OnLaserHit(bool isHit)
    {
        if (isHit)
        {
            TakeDamage(singleLaserDamage);
        }
    }

    private void TakeDamage(float damage)
    {
        canRegenerate = false;
        StopAllCoroutines();
        
        currentDurability = Mathf.Max(0, currentDurability - damage);
        UpdateEmission();

        if (currentDurability <= 0)
        {
            platformCollider.enabled = false;
            StartCoroutine(StartRegeneration());
        }
    }

    private void UpdateEmission()
    {
        float intensityFactor = currentDurability / maxDurability;
        
        // Update emission
        Color emissionColor = initialEmissionColor * intensityFactor;
        lineMaterial.SetColor("_EmissionColor", emissionColor);
        
        // Update alpha
        Color baseColor = lineMaterial.color;
        baseColor.a = 0.5f * intensityFactor;
        lineMaterial.color = baseColor;

        spriteRenderer.enabled = currentDurability > 0.01f;
    }

    private IEnumerator StartRegeneration()
    {
        yield return regenerationDelay;
        canRegenerate = true;
        isRegenerating = true;
    }
}
