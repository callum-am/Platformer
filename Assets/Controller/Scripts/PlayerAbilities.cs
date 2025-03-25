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
    public PlayerStats stats;
    // Start is called before the first frame update
    public GameObject Beam;
    public GameObject Sphere;
    public GameObject Shield;
    public GameObject Teleport;
    //public GameObject Locked1;
    //public GameObject Locked2;
    public Image maskOff;
    public Image maskUtil;
    [HideInInspector]public float cooldown1;
    [HideInInspector]public float cooldown2;
    [HideInInspector]public float dashCooldown;
    [HideInInspector]public bool isCooldown1 = false;
    [HideInInspector]public bool isCooldown2 = false;
    [HideInInspector]public bool isCooldownDash = false;
    public GameObject player;
    
    void Start()
    {
        if (stats.offensiveAbilityVariant == 1)
        {
            Beam.GetComponent<SpriteRenderer>().enabled = true;
            Sphere.GetComponent<SpriteRenderer>().enabled = false;
            cooldown1 = 10f;
        }
        else if (stats.offensiveAbilityVariant == 2)
        {
            Beam.GetComponent<SpriteRenderer>().enabled = false;
            Sphere.GetComponent<SpriteRenderer>().enabled = true;
            cooldown1 = 5f;
        }
        else
        {
            Beam.GetComponent<SpriteRenderer>().enabled = false;
            Sphere.GetComponent<SpriteRenderer>().enabled = false;
            //Locked1.SetActive(true);
        }

        if (stats.utilityAbilityVariant == 1)
        {
            Shield.GetComponent<SpriteRenderer>().enabled = false;
            Teleport.GetComponent<SpriteRenderer>().enabled = true;
            cooldown2 = 7f;
        }
        else if (stats.utilityAbilityVariant == 2)
        {
            Shield.GetComponent<SpriteRenderer>().enabled = true;
            Teleport.GetComponent<SpriteRenderer>().enabled = false;
            cooldown2 = 5f;
        }
        else
        {
            Shield.GetComponent<SpriteRenderer>().enabled = false;
            Teleport.GetComponent<SpriteRenderer>().enabled = false;
            //Locked2.SetActive(true);
        }
        maskOff.fillAmount = 0;
        maskUtil.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Ability1();
        Ability2();
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
}
