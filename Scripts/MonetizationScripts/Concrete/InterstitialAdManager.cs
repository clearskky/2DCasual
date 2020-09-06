using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAdManager : MonoBehaviour
{

    string gameId = "3796861";
    bool testMode = true;

    void Start()
    {
        // Initialize the Ads service:
        Advertisement.Initialize(gameId, testMode);
    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        else
        {
            GameEventManager.Instance.ResumeGameAfterSkippingInterstitialAd();
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }
}