using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BombEnemy : MonoBehaviour
{
    private Vector3 targetPos {get => target.position + Vector3.up;}
    private List<Vector3> path = new List<Vector3>();
    private Transform target;
    private Rigidbody2D rb;
    private EnemyHealth selfHealth;
    private float speed = 5f;
    private float stuckTimer = -1f;
    private Vector3 lastPos;
    private bool isStuck = false;
    private LayerMask PlayerLayer;
    public LayerMask WallLayer;
    public float knockbackForce = 1000f; // Add this variable at the top with other properties
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        selfHealth = GetComponent<EnemyHealth>();
        PlayerLayer = LayerMask.GetMask("Player");
        WallLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable");
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Get direction from enemy to player
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            
            // Get the player controller and apply force
            var playerController = collision.gameObject.GetComponent<PlayerController.PlayerController>();
            if (playerController != null)
            {
                playerController.AddFrameForce(knockbackDirection * knockbackForce);
            }

            // Damage player
            collision.collider.gameObject.GetComponent<PlayerHealth>().Damage(20);
            
            // Ensure bomb is destroyed
            selfHealth.Damage(selfHealth.health);
        }       
    }

    // Update is called once per frame
    void Update()
    {

        if (target == null)
        {
            return;
        }

        var targetDir = targetPos - transform.position;

        //Check for line of sight to target
        if (!Physics2D.Raycast(transform.position, targetDir, Vector3.Distance(transform.position, targetPos), WallLayer))  
        {
            //Clear old path waypoints if there are any
            path.Clear();

            path.Add(targetPos);

            //Debug.Log("Line of sight");
            
        }
        else
        {
            if (path.Count > 0 && Vector2.Distance(path[path.Count - 1], targetPos) > 1f)
            {
                path.Add(targetPos);
                Debug.Log("Adding target to path"+ targetPos);
            }
        }

        //If a path exists (target has crossed line of sight), and the distance between the last waypoint and the target is greater than 1, add the target to the path


        if (path.Count == 0)
        {
            //If there is no path, do nothing
            return;
        }

        //If current position is within 1 units of the target position, remove the target from the path
        if (Vector2.Distance(transform.position, path[0]) < 1f)
        {
            //If the enemy is within 1 units of the first waypoint, remove the waypoint
            path.RemoveAt(0);
            isStuck = false;
        }

    }

    private void FixedUpdate()
    {
        //If no path or target, stop and do nothing
        if(path.Count == 0)
        {
            //Slow down if no target
            rb.drag = 1f;
        }
        else{
        //Rotate towards target
        rb.drag = 0f;
        rb.MoveRotation(Quaternion.LookRotation(Vector3.forward, path[0] - transform.position));
        rb.AddForce(transform.up * speed);
        }
        //Apply Force

        //Prevent overspeeding
        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }

    
    
}
