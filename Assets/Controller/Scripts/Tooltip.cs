using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDescription : MonoBehaviour
{
    public string title;
    public string description;
    public string cost;
    void OnMouseEnter()
    {
        SkillTreeController.instance.ShowTooltip(title, description, cost);
    }

    void OnMouseExit()
    {
        SkillTreeController.instance.HideTooltip();
    }
}
