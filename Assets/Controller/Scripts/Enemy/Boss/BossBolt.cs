using UnityEngine;
using System.Collections;

public class BossBolt : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float rotationSpeed = 5f;
    public float trackingDuration = 2f;
    
    private Transform target;
    private Vector2 launchDirection;
    private bool hasLaunched = false;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0;
        
        // Ignore collisions with enemy layer
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        
        StartCoroutine(TrackAndLaunch());
    }

    void Update()
    {
        if (!hasLaunched && target != null)
        {
            // Track player
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator TrackAndLaunch()
    {
        yield return new WaitForSeconds(trackingDuration);
        
        // Store final direction and launch
        launchDirection = transform.right;
        rb.velocity = launchDirection * projectileSpeed;
        hasLaunched = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Damage the player and destroy the bolt
            collision.gameObject.GetComponent<PlayerHealth>()?.Damage(10);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.Damage(10);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Default") || 
                 collision.gameObject.layer == LayerMask.NameToLayer("Climbable"))
        {
            Destroy(gameObject);
        }
    }
}
