using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

public class PupilFollow : MonoBehaviour
{
    public Transform Pupil;
    public Transform aimDirection;
    public Transform Eyeball;
    public float EyeRadius = 0.001f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 aimDirectionLocal = player.GetComponent<PlayerController.PlayerController>()._frameInput.Mouse;
        Vector3 lookOffset = aimDirectionLocal - Eyeball.position;
        lookOffset = lookOffset.normalized * EyeRadius;
        lookOffset.Scale (new Vector3 (0.1f, 0.05f));
        Pupil.position = Eyeball.position + lookOffset;
    }
}
