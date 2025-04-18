using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using PlayerController;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public GameObject Beam;
    public GameObject Sphere;
    public GameObject Shield;
    public GameObject Teleport;
    public GameObject Dash;
    public Image maskOff;
    public Image maskUtil;
    public Image maskDash;
    [HideInInspector]public float cooldown1;
    [HideInInspector]public float cooldown2;
    [HideInInspector]public float dashCooldown;
    [HideInInspector]public bool isCooldown1 = false;
    [HideInInspector]public bool isCooldown2 = false;
    [HideInInspector]public bool isCooldownDash = false;
    public GameObject player;
    public GameObject Locked1;
    public GameObject Locked2;
    public GameObject ShopDialogue;
    
    void Start()
    {
        
        var config = PlayerConfigManager.Instance.Config;
        dashCooldown = config.dashCooldown;

        ShopDialogue.SetActive(false);

        if (config.offensiveAbilityVariant == 1)
        {
            Beam.GetComponent<SpriteRenderer>().enabled = true;
            Sphere.GetComponent<SpriteRenderer>().enabled = false;
            Locked1.GetComponent<SpriteRenderer>().enabled = false;
            cooldown1 = 10f;
        }
        else if (config.offensiveAbilityVariant == 2)
        {
            Beam.GetComponent<SpriteRenderer>().enabled = false;
            Sphere.GetComponent<SpriteRenderer>().enabled = true;
            Locked1.GetComponent<SpriteRenderer>().enabled = false;
            cooldown1 = 5f;
        }
        else
        {
            Beam.GetComponent<SpriteRenderer>().enabled = false;
            Sphere.GetComponent<SpriteRenderer>().enabled = false;
            Locked1.GetComponent<SpriteRenderer>().enabled = true;
            cooldown1 = 0f;
        }

        if (config.utilityAbilityVariant == 1)
        {
            Shield.GetComponent<SpriteRenderer>().enabled = false;
            Teleport.GetComponent<SpriteRenderer>().enabled = true;
            Locked2.GetComponent<SpriteRenderer>().enabled = false;
            cooldown2 = 7f;
        }
        else if (config.utilityAbilityVariant == 2)
        {
            Shield.GetComponent<SpriteRenderer>().enabled = true;
            Teleport.GetComponent<SpriteRenderer>().enabled = false;
            Locked2.GetComponent<SpriteRenderer>().enabled = false;
            cooldown2 = 5f;
        }
        else
        {
            Shield.GetComponent<SpriteRenderer>().enabled = false;
            Teleport.GetComponent<SpriteRenderer>().enabled = false;
            Locked2.GetComponent<SpriteRenderer>().enabled = true;
        }
        maskOff.fillAmount = 0;
        maskUtil.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var config = PlayerConfigManager.Instance.Config;
        Ability1();
        Ability2();
        DashAbility();

    }
    public void Ability1()
    {
        if (player.GetComponent<PlayerController.PlayerController>()._frameInput.AbilityDown && !isCooldown1)
        {
            isCooldown1 = true;
            maskOff.fillAmount = 1;
        }
        if (isCooldown1)
        {
            maskOff.fillAmount -= 1 / cooldown1 * Time.deltaTime;
            if (maskOff.fillAmount <= 0)
            {
                maskOff.fillAmount = 0;
                isCooldown1 = false;
            }
        }
    }

        public void Ability2()
    {
        if (player.GetComponent<PlayerController.PlayerController>()._frameInput.UtilityDown && !isCooldown2)
        {
            isCooldown2 = true;
            maskUtil.fillAmount = 1;
        }
        if (isCooldown2)
        {
            maskUtil.fillAmount -= 1 / cooldown2 * Time.deltaTime;
            if (maskUtil.fillAmount <= 0)
            {
                maskUtil.fillAmount = 0;
                isCooldown2 = false;
            }
        }
    }

    public void DashAbility()
    {
        if (player.GetComponent<PlayerController.PlayerController>()._frameInput.DashDown && !isCooldownDash)
        {
            Debug.Log("Dash");
            isCooldownDash = true;
            maskDash.fillAmount = 1;
        }
        if (isCooldownDash)
        {
            maskDash.fillAmount -= 1 / dashCooldown * Time.deltaTime;
            if (maskDash.fillAmount <= 0)
            {
                maskDash.fillAmount = 0;
                isCooldownDash = false;
            }
        }
    }

    public void GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ShopEncounter()
    {
        if (ShopDialogue != null)
        {
            ShopDialogue.SetActive(true);
            Cursor.visible = true;
        }
        else
        {
            Debug.Log("ShopDialogue is null");
        }
    }

    public void DeclineShopEncounter()
    {
        if (ShopDialogue != null)
        {
            ShopDialogue.SetActive(false);
        }
        else
        {
            Debug.Log("ShopDialogue is null");
        }
    }

    public void AcceptShopEncounter()
    {
        if (ShopDialogue != null)
        {
            var config = PlayerConfigManager.Instance.Config;
            config.currentRunExperience -= 400;
            player.GetComponent<PlayerHealth>().health = config.maxHealth;
            ShopDialogue.SetActive(false);
        }
        else
        {
            Debug.Log("ShopDialogue is null");
        }
    }
}
