using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    public float targetCameraSize = 10f;
    public float transitionSpeed = 2f;
    public float destroyDelay = 2f;
    public BossController boss;
    
    private Camera mainCamera;
    private GameObject bossOverlay;    // Changed to private
    private GameObject bossHealthBar;  // Changed to private
    private bool isTransitioning = false;
    private float initialCameraSize;

    void Start()
    {
        mainCamera = Camera.main;
        initialCameraSize = mainCamera.orthographicSize;

        // Find UI elements in main camera's canvas
        Canvas mainCanvas = mainCamera.GetComponentInChildren<Canvas>();
        if (mainCanvas != null)
        {
            bossOverlay = mainCanvas.transform.Find("BossOverlay")?.gameObject;
            bossHealthBar = mainCanvas.transform.Find("BossHealthBar")?.gameObject;
            
            // Ensure UI elements are initially hidden
            if (bossOverlay) bossOverlay.SetActive(false);
            if (bossHealthBar) bossHealthBar.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No canvas found in main camera!");
        }
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
            
            // Activate UI elements
            if (bossOverlay) bossOverlay.SetActive(true);
            if (bossHealthBar) bossHealthBar.SetActive(true);
            boss.bossHealthBar = bossHealthBar.GetComponent<Image>();
        }
    }
}
