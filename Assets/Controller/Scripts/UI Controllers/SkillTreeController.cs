using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.InputSystem;

public class SkillTreeController : MonoBehaviour
{
    public static SkillTreeController instance;
    public TextMeshProUGUI AbilityTitle;
    public TextMeshProUGUI AbilityDescription;
    public TextMeshProUGUI AbilityCost;
    public Camera Camera;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        Cursor.visible = true;
    }

    void Update()
    {

    }
    public void ShowTooltip(string title, string description, string cost)
    {
        AbilityTitle.text = title;
        AbilityDescription.text = description;
        AbilityCost.text = cost;
    }
    public void HideTooltip()
    {
        AbilityTitle.text = "";
        AbilityDescription.text = "";
        AbilityCost.text = "";
    }
}
