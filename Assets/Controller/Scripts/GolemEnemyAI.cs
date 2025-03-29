using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemEnemyAI : MonoBehaviour
{
    private float speed = 5f;
    private Rigidbody2D rb;
    private Transform target;
    private LayerMask playerLayer;
    private LayerMask wallLayer;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        wallLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Physics update
    void FixedUpdate()
    {
        
    }

    private Boolean CheckForPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 100f, wallLayer);

        if (hit.collider != null && hit.collider.gameObject.layer == playerLayer)
        {
            return true;
        }else{
            return false;
        }
    }

    private void Patrol()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 5f, wallLayer);

        if (hit.collider != null)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
