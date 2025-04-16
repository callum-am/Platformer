using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSprite : MonoBehaviour
{
    public Wheel wheel;
    private List<Vector3> wheelEyePositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        // Ignore collisions with specified layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("EnemyProjectile"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("PlayerProjectile"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer, true); // Ignore self layer

        IndexWheelPositions();
    }

    private void IndexWheelPositions()
    {
        if (wheel == null) return;

        wheelEyePositions.Clear();
        foreach (Eye eye in wheel.eyes)
        {
            // Store position relative to the SunSprite
            Vector3 relativePosition = transform.InverseTransformPoint(eye.transform.position);
            wheelEyePositions.Add(relativePosition);
            Debug.Log($"Indexed eye position: {relativePosition}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
