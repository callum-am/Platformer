using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin , BackAndForth }

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float projectileLifetime = 2f;

    [Header("Spawner Settings")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float spawnRate = 1f;

    private GameObject spawnedProjectile;
    private float timeSinceLastSpawn = 0f;
    private float rotDirection = 1f;
    private bool isActive = false;
    private Transform spawnRotation;

    // Start is called before the first frame update
    void Start()
    {
        spawnRotation = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isActive) return;  // Only run if spawner is active

        timeSinceLastSpawn += Time.deltaTime;
        if(spawnerType == SpawnerType.Spin) transform.eulerAngles = new Vector3(0f,0f,transform.eulerAngles.z+rotDirection); // Rotate the spawner by rotDirection, determining speed and direction
        if (spawnerType == SpawnerType.BackAndForth) 
        {
            // Map zRotation to [-180, 180] range
            float zRotation = transform.eulerAngles.z;
            if (zRotation > 180f) zRotation -= 360f;

            // Check thresholds and adjust rotDirection
            if (zRotation >= 45f) 
                rotDirection = -1f;
            else if (zRotation <= -45f) 
                rotDirection = 1f;

            Debug.Log(rotDirection.ToString());

            // Apply rotation
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + rotDirection);
        }
        if (timeSinceLastSpawn >= spawnRate)
        {
            SpawnProjectile();
            timeSinceLastSpawn = 0;
        }
    }

    private void SpawnProjectile()
    {
        if(projectilePrefab){
        spawnedProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        spawnedProjectile.GetComponent<Bolt>().speed = projectileSpeed;
        spawnedProjectile.GetComponent<Bolt>().despawnTime = projectileLifetime;
        spawnedProjectile.transform.rotation = transform.rotation;
        }
    }

    public void SetStraightFiring()
    {
        spawnerType = SpawnerType.Straight;
    }

    public void SetSpinFiring()
    {
        spawnerType = SpawnerType.Spin;
    }

    public void StartFiring()
    {
        isActive = true;
        timeSinceLastSpawn = spawnRate; // Allow immediate first shot
    }

    public void StopFiring()
    {
        isActive = false;
        transform.rotation = spawnRotation.rotation; // Reset rotation
    }

    public void SetSpawnRate(float rate)
    {
        spawnRate = rate;
    }
}
