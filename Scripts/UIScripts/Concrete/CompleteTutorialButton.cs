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
        Time.timeScale = 1;
        AudioManager.Instance.PlayButtonHitClip();
        PlayerPrefs.SetInt("tutorialHasBeenCompleted", 1);
        SceneManager.LoadScene("SampleScene");
    }
}
