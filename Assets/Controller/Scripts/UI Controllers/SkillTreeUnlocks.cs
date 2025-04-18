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

    void FixedUpdate()
    {
        var config = PlayerConfigManager.Instance.Config;
        AvailablePoints.text = "Available Points: " + config.currentExperience.ToString();
        TotalPoints.text = "Total Points: " + config.totalExperience.ToString();
    }
    void Update()
    {
        
        var config = PlayerConfigManager.Instance.Config;
        AvailablePoints.text = "Available Points: " + config.currentExperience.ToString();
        TotalPoints.text = "Total Points: " + config.totalExperience.ToString();

        //Offensive Ability Variants

        if (config.offensiveAbilityVariant == 1)
        {
            beamImage.fillAmount = 0;
            sphereImage.fillAmount = 1;
        }else if (config.offensiveAbilityVariant == 2)
        {
            beamImage.fillAmount = 1;
            sphereImage.fillAmount = 0;
        }else if (config.offensiveAbilityVariant == 0)
        {
            beamImage.fillAmount = 1;
            sphereImage.fillAmount = 1;
        }

        //Utility Ability Variants

        if (config.utilityAbilityVariant == 1)
        {
            teleportImage.fillAmount = 0;
            shieldImage.fillAmount = 1;
        }else if (config.utilityAbilityVariant == 2)
        {
            teleportImage.fillAmount = 1;
            shieldImage.fillAmount = 0;
        }else if (config.utilityAbilityVariant == 0)
        {
            teleportImage.fillAmount = 1;
            shieldImage.fillAmount = 1;
        }

        //Basic Attack Variants

        if (config.basicAttackVariant == 1)
        {
            ricochetImage.fillAmount = 0;
            burstImage.fillAmount = 1;
            defaultAttackImage.fillAmount = 1;
        }else if (config.basicAttackVariant == 2)
        {
            ricochetImage.fillAmount = 1;
            burstImage.fillAmount = 0;
            defaultAttackImage.fillAmount = 1;
        }else if (config.basicAttackVariant == 0)
        {
            defaultAttackImage.fillAmount = 0;
            ricochetImage.fillAmount = 1;
            burstImage.fillAmount = 1;
        }

        //Passive Upgrades

        if (config.healthUpgrade1Unlocked)
        {
            healthImage.fillAmount = 0;
        }else{
            healthImage.fillAmount = 1;
        }

        if (config.healthUpgrade2Unlocked)
        {
            healthImage2.fillAmount = 0;
        }else{
            healthImage2.fillAmount = 1;
        }

        if (config.speedUpgradeUnlocked)
        {
            speedImage.fillAmount = 0;
        }else{
            speedImage.fillAmount = 1;
        }

        if (config.dashUpgradeUnlocked)
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
        Debug.Log("beam");
        var config = PlayerConfigManager.Instance.Config;
        if (config.beamUnlocked)
        {
            config.offensiveAbilityVariant = 1;
            config.SaveToFile();
        }else if(!config.beamUnlocked && config.currentExperience >= 1000)
        {
            config.beamUnlocked = true;
            config.currentExperience -= 1000;
            config.offensiveAbilityVariant = 1;
            config.SaveToFile();
        }else if (!config.beamUnlocked && config.currentExperience < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void ResetEXP()
    {
        var config = PlayerConfigManager.Instance.Config;
        config.ResetConfig();
        config.SaveToFile();
    }

    public void Add1000()
    {
        var config = PlayerConfigManager.Instance.Config;
        config.AddExperience(1000);
        config.SaveToFile();
    }

    public void UnlockSphere()
    {
        var config = PlayerConfigManager.Instance.Config;
        if (config.sphereUnlocked)
        {
            config.offensiveAbilityVariant = 2;
            config.SaveToFile();
        }else if(!config.sphereUnlocked && config.currentExperience >= 1000)
        {
            config.sphereUnlocked = true;
            config.currentExperience -= 1000;
            config.offensiveAbilityVariant = 2;
            config.SaveToFile();
        }else if (!config.sphereUnlocked && config.currentExperience < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void UnlockTeleport()
    {
        var config = PlayerConfigManager.Instance.Config;
        if (config.teleportUnlocked)
        {
            config.utilityAbilityVariant = 1;
            config.SaveToFile();
        }else if(!config.teleportUnlocked && config.currentExperience >= 1000)
        {
            config.teleportUnlocked = true;
            config.currentExperience -= 1000;
            config.utilityAbilityVariant = 1;
            config.SaveToFile();
        }else if (!config.teleportUnlocked && config.currentExperience < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void UnlockShield()
    {
        var config = PlayerConfigManager.Instance.Config;
        if (config.shieldUnlocked)
        {
            config.utilityAbilityVariant = 2;
            config.SaveToFile();

        }else if(!config.shieldUnlocked && config.currentExperience >= 1000)
        {
            config.shieldUnlocked = true;
            config.currentExperience -= 1000;
            config.utilityAbilityVariant = 2;
            config.SaveToFile();
        }else if (!config.shieldUnlocked && config.currentExperience < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void UnlockRicochet()
    {
        var config = PlayerConfigManager.Instance.Config;
        if (config.ricochetUnlocked)
        {
            config.basicAttackVariant = 1;
            config.SaveToFile();
        }else if(!config.ricochetUnlocked && config.currentExperience >= 750)
        {
            config.ricochetUnlocked = true;
            config.currentExperience -= 750;
            config.basicAttackVariant = 1;
            config.SaveToFile();
        }else if (!config.ricochetUnlocked && config.currentExperience < 750)
        {
            Debug.Log("Not enough points");
        }
    }

    public void UnlockBurst()
    {
        var config = PlayerConfigManager.Instance.Config;
        if (config.burstUnlocked)
        {
            config.basicAttackVariant = 2;
            config.SaveToFile();
        }else if(!config.burstUnlocked && config.currentExperience >= 1000)
        {
            config.burstUnlocked = true;
            config.currentExperience -= 1000;
            config.basicAttackVariant = 2;
            config.SaveToFile();
        }else if (!config.burstUnlocked && config.currentExperience < 1000)
        {
            Debug.Log("Not enough points");
        }
    }

    public void HealthUpgrade()
    {
        var config = PlayerConfigManager.Instance.Config;
        if (config.healthUpgrade1Unlocked)
        {
            return;
        }else if(!config.healthUpgrade1Unlocked && config.currentExperience >= 500)
        {
            config.healthUpgrade1Unlocked = true;
            config.maxHealth += 25f;
        }else if (!config.healthUpgrade1Unlocked && config.currentExperience <= 500)
        {
            Debug.Log("Not Enough Points");
        }
    }

    public void HealthUpgrade2()
    {
        Debug.Log("Health Upgrade 2");
        var config = PlayerConfigManager.Instance.Config;
        if (config.healthUpgrade2Unlocked)
        {
            return;
        }else if(!config.healthUpgrade2Unlocked && config.currentExperience >= 750)
        {
            config.healthUpgrade2Unlocked = true;
            config.maxHealth += 25f;
        }else if (!config.healthUpgrade2Unlocked && config.currentExperience <= 750)
        {
            Debug.Log("Not Enough Points");
        }
    }

    public void SpeedUpgrade()
    {
        var config = PlayerConfigManager.Instance.Config;
        if (config.speedUpgradeUnlocked)
        {
            return;
        }else if(!config.speedUpgradeUnlocked && config.currentExperience >= 500)
        {
            config.speedUpgradeUnlocked = true;
            config.baseSpeed += 2;
            config.currentExperience -= 500;
            config.SaveToFile();
        }else if (!config.speedUpgradeUnlocked && config.currentExperience < 500)
        {
            Debug.Log("Not enough points");
        }
    }

    public void DashUpgrade()
    {
        var config = PlayerConfigManager.Instance.Config;
        if (config.dashUpgradeUnlocked)
        {
            return;
        }else if(!config.dashUpgradeUnlocked && config.currentExperience >= 750)
        {
            config.dashUpgradeUnlocked = true;
            config.dashCooldown *= 0.8f;
            config.currentExperience -= 750;
            config.SaveToFile();
        }else if (!config.dashUpgradeUnlocked && config.currentExperience < 750)
        {
            Debug.Log("Not enough points");
        }
    }

    public void DefaultAttack()
    {
        PlayerConfigManager.Instance.Config.basicAttackVariant = 0;
        PlayerConfigManager.Instance.Config.SaveToFile();
    }

}
