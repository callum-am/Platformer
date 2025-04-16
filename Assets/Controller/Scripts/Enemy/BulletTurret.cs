using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTurret : MonoBehaviour
{
    public GameObject projectilePrefab;
    private GameObject spawnedProjectile;
    public float projectileLifetime = 5f;
    public LayerMask WallLayer;
    public Transform firePoint;
    public Transform mountPoint;
    public float fireRate = 1f;

    public Transform target;
    private Vector3 targetPos {get => target.position + Vector3.up;}
    public EnemyHealth selfHealth;
    private float nextFireTime = 0f;
    public float projectileSpeed = 10f;
    public float rotationSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        WallLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable");
        selfHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 directionToTarget = (targetPos - mountPoint.position).normalized;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        
        // Smoothly rotate towards target
        mountPoint.rotation = Quaternion.Slerp(
            mountPoint.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );

        // Check line of sight and fire if possible
        if (!Physics2D.Raycast(firePoint.position, directionToTarget, 
            Vector3.Distance(firePoint.position, targetPos), WallLayer)) 
        {
            if (Time.time >= nextFireTime)
            {
                SpawnProjectile();
                nextFireTime = Time.time + 1f/fireRate;
            }
        }
    }

    private void SpawnProjectile()
    {
        if(projectilePrefab){
        spawnedProjectile = Instantiate(projectilePrefab, firePoint.position, mountPoint.rotation);
        spawnedProjectile.GetComponent<Bolt>().speed = projectileSpeed;
        spawnedProjectile.GetComponent<Bolt>().despawnTime = projectileLifetime;
        spawnedProjectile.transform.rotation = mountPoint.rotation;
        }
    }
}
