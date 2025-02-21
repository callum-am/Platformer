using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class LightBall : MonoBehaviour
{
    public float speed;
    public float rotation;
    public float despawnTime; //Define the time it takes for the LightBall to despawn
    private float timer = 0f;
    private enum fireMode { straight, bouncing };
    private Vector2 spawnPoint;
    public Rigidbody2D rb;
    public PhysicsMaterial2D bouncy;
    public PhysicsMaterial2D straight;


    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > despawnTime)
        {
            Destroy(this.gameObject);
        }
        timer += Time.deltaTime;
    }

    
}


