using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using PlayerController;
using UnityEngine.UI;
using TMPro;

public class SkillTreeUnlocks : MonoBehaviour
{
    public TextMeshProUGUI AvailablePoints;
    public TextMeshProUGUI TotalPoints;
    public PlayerStats Stats;
    public Image beamImage;
    public Image sphereImage;
    public Image teleportImage;
    public Image shieldImage;
    public Image defaultAttackImage;
    public Image ricochetImage;
    public Image burstImage;
    public Image healthImage;
    public Image healthImage2;
    public Image speedImage;
    public Image dashImage;

    void Update()
    {
        AvailablePoints.text = "Available Points: " + Stats.spentPoints.ToString();
        TotalPoints.text = "Total Points: " + Stats.totalExperience.ToString();

        //Offensive Ability Variants

        if (Stats.offensiveAbilityVariant == 1)
        {
            beamImage.fillAmount = 1;
            sphereImage.fillAmount = 0;
        }else if (Stats.offensiveAbilityVariant == 2)
        {
            beamImage.fillAmount = 0;
            sphereImage.fillAmount = 1;
        }else if (Stats.offensiveAbilityVariant == 0)
        {
            beamImage.fillAmount = 1;
            sphereImage.fillAmount = 1;
        }

        //Utility Ability Variants

        if (Stats.utilityAbilityVariant == 1)
        {
            teleportImage.fillAmount = 1;
            shieldImage.fillAmount = 0;
        }else if (Stats.utilityAbilityVariant == 2)
        {
            teleportImage.fillAmount = 0;
            shieldImage.fillAmount = 1;
        }else if (Stats.utilityAbilityVariant == 0)
        {
            teleportImage.fillAmount = 1;
            shieldImage.fillAmount = 1;
        }

        //Basic Attack Variants

        if (Stats.basicAttackVariant == 1)
        {
            ricochetImage.fillAmount = 0;
            burstImage.fillAmount = 1;
            defaultAttackImage.fillAmount = 1;
        }else if (Stats.basicAttackVariant == 2)
        {
            ricochetImage.fillAmount = 1;
            burstImage.fillAmount = 0;
            defaultAttackImage.fillAmount = 1;
        }else if (Stats.basicAttackVariant == 0)
        {
            defaultAttackImage.fillAmount = 0;
            ricochetImage.fillAmount = 1;
            burstImage.fillAmount = 1;
        }

        //Passive Upgrades

        if (Stats.healthUpgrade1Unlocked)
        {
            healthImage.fillAmount = 0;
        }else{
            healthImage.fillAmount = 1;
        }

        if (Stats.healthUpgrade2Unlocked)
        {
            healthImage2.fillAmount = 0;
        }else{
            healthImage2.fillAmount = 1;
        }

        if (Stats.speedUpgradeUnlocked)
        {
            speedImage.fillAmount = 0;
        }else{
            speedImage.fillAmount = 1;
        }

        if (Stats.dashUpgradeUnlocked)
        {
            dashImage.fillAmount = 0;
        }else{
            dashImage.fillAmount = 1;
        }
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Menu");
    }
    public void UnlockBeam()
    {
        if (Stats.beamUnlocked)
        {
            Stats.offensiveAbilityVariant = 1;
        }else if(Stats.beamUnlocked == false && Stats.spentPoints >= 1000)
        {
            Stats.beamUnlocked = true;
            Stats.spentPoints -= 1000;
            Stats.offensiveAbilityVariant = 1;
        }else if (Stats.beamUnlocked == false && Stats.spentPoints < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void UnlockSphere()
    {
        if (Stats.sphereUnlocked)
        {
            Stats.offensiveAbilityVariant = 2;
        }else if(Stats.sphereUnlocked == false && Stats.spentPoints >= 1000)
        {
            Stats.sphereUnlocked = true;
            Stats.spentPoints -= 1000;
            Stats.offensiveAbilityVariant = 2;
        }else if (Stats.sphereUnlocked == false && Stats.spentPoints < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void UnlockTeleport()
    {
        if (Stats.teleportUnlocked)
        {
            Stats.utilityAbilityVariant = 1;
        }else if(Stats.teleportUnlocked == false && Stats.spentPoints >= 1000)
        {
            Stats.teleportUnlocked = true;
            Stats.spentPoints -= 1000;
            Stats.utilityAbilityVariant = 1;
        }else if (Stats.teleportUnlocked == false && Stats.spentPoints < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void UnlockShield()
    {
        if (Stats.shieldUnlocked)
        {
            Stats.utilityAbilityVariant = 2;

        }else if(Stats.shieldUnlocked == false && Stats.spentPoints >= 1000)
        {
            Stats.shieldUnlocked = true;
            Stats.spentPoints -= 1000;
            Stats.utilityAbilityVariant = 2;
        }else if (Stats.shieldUnlocked == false && Stats.spentPoints < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void UnlockRicochet()
    {
        if (Stats.ricochetUnlocked)
        {
            Stats.basicAttackVariant = 1;
        }else if(Stats.ricochetUnlocked == false && Stats.spentPoints >= 750)
        {
            Stats.ricochetUnlocked = true;
            Stats.spentPoints -= 750;
            Stats.basicAttackVariant = 1;
        }else if (Stats.ricochetUnlocked == false && Stats.spentPoints < 750)
        {
            Debug.Log("Not enough points");
        }
    }

    public void UnlockBurst()
    {
        if (Stats.burstUnlocked)
        {
            Stats.basicAttackVariant = 2;
        }else if(Stats.burstUnlocked == false && Stats.spentPoints >= 1000)
        {
            Stats.burstUnlocked = true;
            Stats.spentPoints -= 1000;
            Stats.basicAttackVariant = 2;
        }else if (Stats.burstUnlocked == false && Stats.spentPoints < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void HealthUpgrade()
    {

    }

    public void HealthUpgrade2()
    {

    }

    public void SpeedUpgrade()
    {
        if (Stats.speedUpgradeUnlocked)
        {
            return;
        }else if(Stats.speedUpgradeUnlocked == false && Stats.spentPoints >= 500)
        {
            Stats.speedUpgradeUnlocked = true;
            Stats.BaseSpeed += 2;
            Stats.spentPoints -= 500;
        }else if (Stats.speedUpgradeUnlocked == false && Stats.spentPoints < 500)
        {
            Debug.Log("Not enough points");
        }
    }

    public void DashUpgrade()
    {
        if (Stats.dashUpgradeUnlocked)
        {
            return;
        }else if(Stats.dashUpgradeUnlocked == false && Stats.spentPoints >= 750)
        {
            Stats.dashUpgradeUnlocked = true;
            Stats.DashCooldown = Stats.DashCooldown * 0.8f;
            Stats.spentPoints -= 750;
        }else if (Stats.dashUpgradeUnlocked == false && Stats.spentPoints < 750)
        {
            Debug.Log("Not enough points");
        }
    }

    public void DefaultAttack()
    {
        Stats.basicAttackVariant = 0;
    }

}
