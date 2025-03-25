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
        gameObject.SetActive(false);
    }

    void Update()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
        transform.position = new Vector3 (Camera.ScreenToWorldPoint(mousePos).x, Camera.ScreenToWorldPoint(mousePos).y, 5);
    }

    public void ShowTooltip(string title, string description, string cost)
    {
        AbilityTitle.text = title;
        AbilityDescription.text = description;
        AbilityCost.text = cost;
        gameObject.SetActive(true);
    }
    public void HideTooltip()
    {
        gameObject.SetActive(false);
        AbilityTitle.text = "";
        AbilityDescription.text = "";
        AbilityCost.text = "";
    }
}
