using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    private bool isImmune = false;
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {      
    }

    public void Damage(float damageAmount)
    {
        if (isImmune) return;

        health -= damageAmount;

        if (health <= 0)
        {
            health = 0;
            Destroy(this.gameObject);
        }
    }

    public void SetImmune()
    {
        isImmune = true;
    }

    public void RemoveImmune()
    {
        isImmune = false;
    }

}
