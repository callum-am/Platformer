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
    //private Health selfHealth;
    private float speed = 5f;
    private float stuckTimer = -1f;
    private Vector3 lastPos;
    private bool isStuck = false;
    private LayerMask PlayerLayer;
    public LayerMask WallLayer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //selfHealth = GetComponent<Health>();
        PlayerLayer = LayerMask.GetMask("Player");
        WallLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable");
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Damage Player
            //collision.collider.gameObject.GetComponent<PlayerHealth>().Damage(20);
            //Ensure bomb is destroyed
            //selfHealth.Damage();
        }       
    }

    // Update is called once per frame
    void Update()
    {

        if (target == null)
        {
            Debug.Log("No target found");
            return;
        }

        var targetDir = targetPos - transform.position;
        
        
        //Debug.Log ("Target position: " + target.position);

        //Check for line of sight to target
        //if (!Physics.Raycast(transform.position, targetPos, out RaycastHit hit, 100f) && !Physics.CheckSphere(transform.position + targetPos, 0.5f, WallLayer))
        //if (!Physics2D.CircleCast(transform.pos, 0.5f, Vector3.Distance(transform.position, targetPos), WallLayer) && !Physics.CheckSphere(transform.position + targetPos, 0.5f, WallLayer))
        //if (!Physics2D.Raycast(transform.position, targetPos, 100f, WallLayer) && !Physics2D.CheckSphere(transform.position + targetPos, 0.5f, WallLayer)) 
        //if (!Physics.Linecast(transform.position, targetPos, out RaycastHit hit, WallLayer))

        //if (Physics.SphereCast(new Ray (transform.position, targetPos), 0.5f, Vector3.Distance(transform.position, targetPos), WallLayer))     

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
            return;
        }

        //If a path exists (target has crossed line of sight), and the distance between the last waypoint and the target is greater than 1, add the target to the path


        if (path.Count == 0)
        {
            //If there is no path, do nothing
            return;
        }

        //If current position is within 0.5 units of the target position, remove the target from the path
        if (Vector2.Distance(transform.position, path[0]) < 0.5f)
        {
            //If the enemy is within 0.5 units of the first waypoint, remove the waypoint
            path.RemoveAt(0);
            isStuck = false;
        }
        //var pathDir = path[0] - transform.position;
        //Debug.Log("Path direction: " + pathDir);
        //Debug.DrawRay(transform.position, pathDir, Color.red, Vector3.Distance(transform.position, path[0]));
    }

    private void FixedUpdate()
    {
        //If no path or target, stop and do nothing
        if(path.Count == 0)
        {
            //Slow down if no target
            rb.drag = 1f;
            return;
        }
        else{
        //var pathDir = Quaternion.LookRotation(Vector3.forward, path[0] - transform.position).normalized;
        //Rotate towards target
        rb.drag = 0f;
        rb.MoveRotation(Quaternion.LookRotation(Vector3.forward, path[0] - transform.position));
        }
        //Apply Force
        rb.AddForce(transform.up * speed);
        //Debug.Log("Attempting to move to target: " + path[0]);

        //Prevent overspeeding
        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }

        var distance = Vector3.Distance(lastPos, rb.position);
        lastPos = rb.position;

/*         if (distance < Time.fixedDeltaTime)
        {
            if (stuckTimer < 0 ) stuckTimer = Time.time;

            if (Time.time > stuckTimer + 1f)
            {
                var randomCircle = Random.insideUnitCircle * 4f;
                var randomPos = transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);

                if (!Physics.CheckSphere(randomPos, 0.5f, WallLayer))
                {
                    path.Insert(0, randomPos);
                    isStuck = true;
                } else {
                    path[0] = randomPos;
                }
            stuckTimer = -1f;
            }

        } */
    }

    
    
}
