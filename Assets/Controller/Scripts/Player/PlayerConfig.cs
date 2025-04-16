using System;
using UnityEngine;
using System.IO;

[Serializable]
public class PlayerConfig
{
    // Progress/Unlocks
    public float dashCooldown = 5f;
    public float baseSpeed = 9f;
    public float currentRunExperience;
    public float currentExperience;
    public float totalExperience;
    public float spentPoints;

    // Ability Unlocks
    public bool beamUnlocked;
    public bool sphereUnlocked;
    public bool teleportUnlocked;
    public bool shieldUnlocked;
    public bool ricochetUnlocked;
    public bool burstUnlocked;

    // Upgrade Unlocks
    public bool healthUpgrade1Unlocked;
    public bool healthUpgrade2Unlocked;
    public bool speedUpgradeUnlocked;
    public bool dashUpgradeUnlocked;
    public float maxHealth = 100f;

    // Current Loadout
    public float basicAttackVariant;
    public float offensiveAbilityVariant;
    public float utilityAbilityVariant;

    private static string SavePath => Path.Combine(Application.persistentDataPath, "playerconfig.json");

    // Constructor to initialize default values
    public PlayerConfig()
    {
        // Set default values
        currentExperience = 0f;
        totalExperience = 0f;
        spentPoints = 0f;
        
    }

    // Save configuration to file
    public void SaveToFile()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(SavePath, json);
    }

    // Load configuration from file
    public static PlayerConfig LoadFromFile()
    {
        if (!File.Exists(SavePath))
        {
            return new PlayerConfig();
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<PlayerConfig>(json);
    }
    // Method to reset the config
    public void ResetConfig()
    {
        currentExperience = 0f;
        totalExperience = 0f;
        spentPoints = 0f;
        beamUnlocked = false;
        sphereUnlocked = false;
        teleportUnlocked = false;
        shieldUnlocked = false;
        ricochetUnlocked = false;
        burstUnlocked = false;
        healthUpgrade1Unlocked = false;
        healthUpgrade2Unlocked = false;
        speedUpgradeUnlocked = false;
        dashUpgradeUnlocked = false;
        maxHealth = 100f;

        SaveToFile();
    }
    public void addRunExperience(float amount)
    {
        currentRunExperience += amount;
        SaveToFile(); // Auto-save when modified
    }   
    // Methods to modify config values
    public void AddExperience(float amount)
    {
        currentExperience += amount;
        totalExperience += amount;
        SaveToFile(); // Auto-save when modified
    }

    public void SpendPoints(float amount)
    {
        spentPoints += amount;
        currentExperience -= amount;
        SaveToFile();
    }

    public void UnlockAbility(string abilityName)
    {
        switch(abilityName.ToLower())
        {
            case "beam": beamUnlocked = true; break;
            case "sphere": sphereUnlocked = true; break;
            case "teleport": teleportUnlocked = true; break;
            case "shield": shieldUnlocked = true; break;
            case "ricochet": ricochetUnlocked = true; break;
            case "burst": burstUnlocked = true; break;
        }
        SaveToFile();
    }

    public void SetLoadout(float basic, float offensive, float utility)
    {
        basicAttackVariant = basic;
        offensiveAbilityVariant = offensive;
        utilityAbilityVariant = utility;
        SaveToFile();
    }

    // Get current config instance
    private static PlayerConfig _current;
    public static PlayerConfig Current
    {
        get
        {
            if (_current == null)
                _current = LoadFromFile();
            return _current;
        }
    }
}
