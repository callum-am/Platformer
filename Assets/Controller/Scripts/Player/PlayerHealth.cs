using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;
    private bool isImmune = false;
    private int immunityFramesRemaining = 0;
    private const int DAMAGE_IMMUNITY_FRAMES = 10;
    private PlayerAbilities CooldownManager;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = PlayerConfigManager.Instance.Config.maxHealth;
        health = maxHealth;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        CooldownManager = GameObject.FindObjectOfType<PlayerAbilities>();
        CooldownManager.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
        
        // Handle immunity frames
        if (immunityFramesRemaining > 0)
        {
            isImmune = true;
            immunityFramesRemaining--;
        }
        else
        {
            isImmune = false;
        }
    }

    public void Damage(float damageAmount)
    {
        if (isImmune) return;

        health -= damageAmount;
        immunityFramesRemaining = DAMAGE_IMMUNITY_FRAMES;

        if (health <= 0)
        {
            health = 0;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Death Screen");
        }
    }

    public void GrantImmunityFrames(int frames)
    {
        immunityFramesRemaining = Mathf.Max(immunityFramesRemaining, frames);
    }

    public bool IsImmune()
    {
        return isImmune;
    }
}
