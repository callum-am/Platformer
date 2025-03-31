using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public float patrolSpeed = 5f;
    public float detectionRange = 0.1f;
    public float knockbackForce = 500f;
    public float knockbackDelay = 0.5f;
    public float knockbackCooldown = 5f; 
    public LayerMask playerLayer;
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isInteractingWithPlayer = false;
    private bool isOnCooldown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isInteractingWithPlayer || isOnCooldown) return;

        if (CheckForPlayerInRange())
        {
            StartCoroutine(HandlePlayerInteraction());
        }
        else
        {
            Patrol();
        }
    }

    private bool CheckForPlayerInRange()
    {
        //Debug.Log("Detection Range: " + detectionRange);
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        return player != null;
    }

    private IEnumerator HandlePlayerInteraction()
    {
        isInteractingWithPlayer = true;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(knockbackDelay);

        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        if (player != null)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * knockbackForce);
            }
        }

        isInteractingWithPlayer = false;
        StartCoroutine(StartKnockbackCooldown());
    }

    private IEnumerator StartKnockbackCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(knockbackCooldown);
        isOnCooldown = false;
    }

    private void Patrol()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, wallLayer);

        if (hit.collider != null)
        {
            Flip();
        }

        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * patrolSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Debug.Log("Gizmo Detection Range: " + detectionRange); // Debug the Gizmo range
    }
}
