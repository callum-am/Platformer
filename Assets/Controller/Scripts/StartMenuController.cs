using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public void TutorialStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }

    public void GameStart()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void SkillTree()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SkillTree");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
