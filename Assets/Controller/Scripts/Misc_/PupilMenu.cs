using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

public class PupilMenu : MonoBehaviour
{
    public Transform Pupil;
    public Transform Eyeball;
    public float EyeRadius = 0.001f;
    public Vector3 lookOffset;

    public bool initialPass = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!initialPass)
        {
            initialPass = true;
            StartCoroutine(RandomlyMovePupil());
        }
    }

    private IEnumerator RandomlyMovePupil()
    {
        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 2; 
            randomDirection.z = 0; 
            Vector3 targetPosition = Eyeball.position + randomDirection * EyeRadius;
            Pupil.position = Vector3.Lerp(Pupil.position, targetPosition, 0.1f);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
