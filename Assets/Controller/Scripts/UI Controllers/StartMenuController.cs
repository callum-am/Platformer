using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartMenuController : MonoBehaviour
{
    private const int MAX_DUNGEON_LENGTH = 20;
    private const int MIN_DUNGEON_LENGTH = 6;

    [Header("Input Popup")]
    public GameObject inputPopup;
    public TMP_InputField seedInput;
    public TMP_InputField lengthInput;
    public Button confirmButton;
    public Button cancelButton;

    private bool IsPopupActive => inputPopup != null && inputPopup.activeSelf;

    void Start()
    {
        // Hide popup initially
        if (inputPopup != null) inputPopup.SetActive(false);

        // Add listeners to buttons
        if (confirmButton != null)
            confirmButton.onClick.AddListener(ConfirmInput);
        if (cancelButton != null)
            cancelButton.onClick.AddListener(() => inputPopup.SetActive(false));

        // Add input validation
        if (lengthInput != null)
        {
            lengthInput.onValueChanged.AddListener(ValidateLengthInput);
        }
    }

    private void ValidateLengthInput(string value)
    {
        // Don't validate empty or in-progress numbers
        if (string.IsNullOrEmpty(value)) return;

        // Only validate if it's a complete number
        if (int.TryParse(value, out int length))
        {
            if (length > MAX_DUNGEON_LENGTH)
            {
                lengthInput.text = MAX_DUNGEON_LENGTH.ToString();
            }
            else if (length < MIN_DUNGEON_LENGTH && value.Length >= 2) // Only enforce minimum if user has finished typing
            {
                lengthInput.text = MIN_DUNGEON_LENGTH.ToString();
            }
        }
        else
        {
            lengthInput.text = MIN_DUNGEON_LENGTH.ToString();
        }
    }

    public void TutorialStart()
    {
        if (IsPopupActive) return;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Controls");
    }

    private void ConfirmInput()
    {
        if (int.TryParse(seedInput.text, out int seed) && 
            int.TryParse(lengthInput.text, out int length))
        {
            length = Mathf.Clamp(length, MIN_DUNGEON_LENGTH, MAX_DUNGEON_LENGTH);
            SetSeed(seed, length);
            Debug.Log($"Seed: {seed}, Length: {length}");
            inputPopup.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
        }
        else
        {
            Debug.LogWarning("Invalid input values");
            // Optionally show error message to user
        }
    }

    public void GameStart()
    {
        if (IsPopupActive) return;
        if (inputPopup != null)
        {
            inputPopup.SetActive(true);
            // Set default values
            seedInput.text = Random.Range(1, 9999).ToString();
            lengthInput.text = "10";
        }
    }

    public void SkillTree()
    {
        if (IsPopupActive) return;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SkillTree");
    }

    public void ExitGame()
    {
        if (IsPopupActive) return;
        Application.Quit();
    }

    public void SetSeed(int seed, int length)
    {
        PlayerConfigManager.Instance.Config.seed = seed;
        PlayerConfigManager.Instance.Config.length = length;
        PlayerConfigManager.Instance.Config.SaveToFile();
    }
}
