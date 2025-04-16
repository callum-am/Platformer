using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBounce : MonoBehaviour
{
    public Transform Eye;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(Time.time) * 0.5f;
        Eye.localPosition = new Vector3(Eye.localPosition.x, y, Eye.localPosition.z);
    }
}
