using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemEnemyAI : MonoBehaviour
{
    private float speed = 25f;
    private Rigidbody2D rb;
    private Vector3 originPos {get => transform.position + Vector3.up;}
    private LayerMask playerLayer;
    private LayerMask wallLayer;
    private Boolean isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        wallLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable") | ~LayerMask.GetMask("Enemy") | LayerMask.GetMask("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Physics update
    void FixedUpdate()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100f, playerLayer);
        if (hit.collider != null)
        {
            ChasePlayer();
            //Debug.Log("Chasing player");
            Debug.Log(hit.collider.gameObject.layer);
            return;
        } else
        {
            Patrol();
            return;
        }
    }

    private bool CheckForPlayer()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        Debug.DrawRay(transform.position, direction * 100f, Color.red); 

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100f, playerLayer); // Corrected Raycast
        if (hit.collider != null)
        {
            Debug.Log("Player detected");
            return true;
        }
        else
        {
            Debug.Log("No Player detected");
            return false;
        }
    }

    private void Patrol()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(originPos, direction, 2f, wallLayer);

        if (hit.collider != null && isFacingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = !isFacingRight;
        }
        else if (hit.collider != null && !isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = !isFacingRight;
        }
        rb.AddForce(direction * speed);
    }

    private void ChasePlayer()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        rb.AddForce(direction * (speed*1.5f));
    }
}
