using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 10);
        Physics2D.IgnoreLayerCollision(10, 7);
        Physics2D.IgnoreLayerCollision(10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreLayerCollision(7, 10);
        Physics2D.IgnoreLayerCollision(10, 7);
        Physics2D.IgnoreLayerCollision(10, 10);
    }
}
