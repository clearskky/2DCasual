using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardedAdButton : MonoBehaviour, IButtonController
{
    public RewardedAdManager rewardedAdManager;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        PlayerPrefs.SetInt("sessionCounter", 0);
        AudioManager.Instance.musicAudioSource.Stop();
        AudioManager.Instance.sfxAudioSource.Stop();
        rewardedAdManager.ShowRewardedVideo();
    }
}
