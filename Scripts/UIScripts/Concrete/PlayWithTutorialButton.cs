﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayWithTutorialButton : MonoBehaviour
{
    public InterstitialAdManager adManager;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        AudioManager.Instance.PlayButtonHitClip();
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("tutorialHasBeenCompleted", 0) == 1)
        {
            PlayerPrefs.SetInt("sessionCounter", PlayerPrefs.GetInt("sessionCounter", 0) + 1); // If the player hasn't reached session count 5, increment it
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.HowToPlay);
        }
        
    }
}
