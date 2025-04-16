using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public float targetCameraSize = 10f;
    public float transitionSpeed = 2f;
    public float destroyDelay = 2f;
    public BossController boss;
    
    private Camera mainCamera;
    private bool isTransitioning = false;
    private float initialCameraSize;


    void Start()
    {
        mainCamera = Camera.main;
        initialCameraSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        if (isTransitioning)
        {
            // Smoothly interpolate camera size
            mainCamera.orthographicSize = Mathf.Lerp(
                mainCamera.orthographicSize, 
                targetCameraSize, 
                transitionSpeed * Time.deltaTime
            );

            // Stop when we're very close to target
            if (Mathf.Abs(mainCamera.orthographicSize - targetCameraSize) < 0.01f)
            {
                mainCamera.orthographicSize = targetCameraSize;
                isTransitioning = false;
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTransitioning = true;
            boss.StartBossFight();
        }
    }
}
