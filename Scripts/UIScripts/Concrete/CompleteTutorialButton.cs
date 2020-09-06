using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompleteTutorialButton : MonoBehaviour, IButtonController
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        AudioManager.Instance.PlayButtonHitClip();
        PlayerPrefs.SetInt("sessionCounter", PlayerPrefs.GetInt("sessionCounter", 0) + 1); // If the player hasn't reached session count 5, increment it
        Time.timeScale = 1;
        PlayerPrefs.SetInt("tutorialHasBeenCompleted", 1);
        SceneManager.LoadScene("SampleScene");
    }
}
