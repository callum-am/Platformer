using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.backgroundColor = new Color(15/255f, 12/255f, 7/255f, 0f);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        } else{
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
        
    }
}
