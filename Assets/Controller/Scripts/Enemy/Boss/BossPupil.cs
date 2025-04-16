using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPupil : MonoBehaviour
{
    private Transform target;
    private Vector3 startPosition;
    public float moveSpeed = 5f;
    public float maxRadius = 0.5f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPosition = transform.localPosition;
    }

    void Update()
    {
        if (target == null) return;

        // Get direction to player
        Vector3 directionToTarget = target.position - transform.parent.TransformPoint(startPosition);
        
        // Calculate target position within radius
        Vector3 targetPosition = Vector3.ClampMagnitude(directionToTarget, maxRadius);
        
        // Convert to local space
        Vector3 localTargetPosition = transform.parent.InverseTransformVector(targetPosition);
        
        // Move towards target position
        transform.localPosition = Vector3.Lerp(
            transform.localPosition, 
            startPosition + localTargetPosition, 
            moveSpeed * Time.deltaTime
        );
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize movement radius
        Gizmos.color = Color.yellow;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(transform.parent.TransformPoint(startPosition), maxRadius);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.parent.TransformPoint(transform.localPosition), maxRadius);
        }
    }
}
