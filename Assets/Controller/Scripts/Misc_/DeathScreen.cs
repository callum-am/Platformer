using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    public TextMeshProUGUI earnedExperience;
    public TextMeshProUGUI totalExperience;
    public TextMeshProUGUI currentExperience;
    // Start is called before the first frame update
    void Start()
    {
        var config = PlayerConfigManager.Instance.Config;
        Cursor.visible = true;
        earnedExperience.text = "Experience Earned This Run: " + config.currentRunExperience.ToString();
        currentExperience.text = "Current Experience: " + config.currentExperience.ToString();
        totalExperience.text = "Total Experience: " + config.totalExperience.ToString();
        config.AddExperience(config.currentRunExperience);
        config.currentRunExperience = 0;
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Menu");
    }
}
