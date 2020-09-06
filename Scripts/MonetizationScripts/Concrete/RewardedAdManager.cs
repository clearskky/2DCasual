using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedAdManager : MonoBehaviour, IUnityAdsListener
{
    string gameId = "3796861";
    bool testMode = true;

    //public Button rewardedAdButton;
    public Castle castle;

    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    // ------------------------------------ Rewarded ads block ----------------------------
    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo");
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        //rewardedAdButton.enabled = true;
        Debug.Log("Ad is ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("The ad started playing.");
        AudioManager.Instance.musicAudioSource.Stop();
        Time.timeScale = 0;
        //rewardedAdButton.enabled = true;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            GameEventManager.Instance.ResumeGameAndHealCastleAfterAd(50);
        }
        else if (showResult == ShowResult.Skipped)
        {
            GameEventManager.Instance.ResumeGameAfterSkippingInterstitialAd();
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnDisable()
    {
        Advertisement.RemoveListener(this);
    }

    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

    // ---------------------------- Rewarded Ads End ----------------------------------------
}
