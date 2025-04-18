using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSprite : MonoBehaviour
{
    public Wheel wheel;
    public List<Vector3> wheelEyePositions = new List<Vector3>();  // Changed to public
    public float eyePositionScaleFactor = 0.5f; // Add this variable to control how close positions are to center (0-1)
    public float moveSpeed = 2f;
    public float approachDelay = 1f;
    private Transform player;
    private CircleCollider2D[] circleColliders;
    private bool isMovingToPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        // Ignore collisions with specified layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("EnemyProjectile"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("PlayerProjectile"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer, true); // Ignore self layer

        IndexWheelPositions();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        circleColliders = GetComponents<CircleCollider2D>();
    }

    private void IndexWheelPositions()
    {
        if (wheel == null) return;

        wheelEyePositions.Clear();
        foreach (Eye eye in wheel.eyes)
        {
            // Store position relative to the SunSprite
            Vector3 relativePosition = transform.InverseTransformPoint(eye.transform.position);
            // Scale the position closer to center by reducing the magnitude
            Vector3 scaledPosition = relativePosition * eyePositionScaleFactor;
            wheelEyePositions.Add(scaledPosition);
            Debug.Log($"Indexed eye position: Original={relativePosition}, Scaled={scaledPosition}");
        }
    }

    public void BeginApproachingPlayer()
    {
        if (!isMovingToPlayer)
        {
            StartCoroutine(ApproachPlayerSequence());
        }
    }

    private IEnumerator ApproachPlayerSequence()
    {
        isMovingToPlayer = true;
        
        // Wait for lasers to lock on
        yield return new WaitForSeconds(approachDelay);

        // Disable colliders
        foreach (var collider in circleColliders)
        {
            collider.enabled = false;
        }

        // Move to player
        while (player != null && Vector2.Distance(transform.position, player.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Notify wheel to start increasing laser intensity
        wheel.StartLaserIntensityIncrease();
        isMovingToPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
