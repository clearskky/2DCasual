﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEventManager : MonoBehaviour, IManager
{
    private static GameEventManager _instance;

    public static GameEventManager Instance
    {
        get { return _instance; }
    }

    public Castle castle;
    public InterstitialAdManager interstitialAdManager;

    [Header("Powerup Buttons")]
    public Button magmaCleaverButton, bladeworksButton;

    public int score, maxMana, currentMana;
    public Transform txtScore;
    public Transform pfab_txtScoreIncrease;

    public Transform manaBarTransform;
    private IHealthBar manaBar;
    public Transform pfab_txtManaIncrease;

    public Text hiScore;
    private int highScore;
    private Text scoreText; // The score text on the top left corner
    
    private Transform powerupInstance;
    
    [Header("Unlimited Bladeworks")] public Transform pfab_bladeworksPortal;
    public int bladeworksManaCost;


    [Header("Magma Cleaver")] public Transform pfab_magmaCleaver;
    public int magmaCleaverManaCost;

    [Header("Jump Boost")] public Transform pfab_jumpBoostPopup;
    public int jumpBoostManaCost, maxBoostedPlayerJumpCount;

    private Transform scorePopup;
    private Transform manaPopup;
    private Transform jumpBoostPopup;

    private bool isGamePaused;

    [SerializeField] private Color notEnoughManaPowerupButtonColor;
   [SerializeField] private Color normalPowerupButtonColor;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        Time.timeScale = 1;
        scoreText = txtScore.GetComponent<Text>();
        manaBar = manaBarTransform.GetComponent<ManaBar>();
        powerupInstance = null;
        isGamePaused = false;

        highScore = PlayerPrefs.GetInt("hiScore", 0);
        hiScore.text = highScore.ToString();
        
        StartInterstitialAd();
        AdjustPowerupButtonVisibility();
    }

    private void StartInterstitialAd()
    {
        if (PlayerPrefs.GetInt("sessionCounter", 0) >= 4)
        {
            isGamePaused = true;
            Time.timeScale = 0;
            interstitialAdManager.ShowInterstitialAd();
        }
        else
        {
            isGamePaused = false;
            Time.timeScale = 1;
        }
    }

    public IEnumerator FreezeTime(float freezeDuration)
    {
        Time.timeScale = 0.01f;
        float freezeEndTime = Time.realtimeSinceStartup + freezeDuration;
        while (Time.realtimeSinceStartup < freezeEndTime)
        {
            yield return 0;
        }

        Time.timeScale = 1;
    }

    public void IncreaseScore(int earnedScore)
    {
        score += earnedScore;
        scoreText.text = score.ToString();

        InstantiateAddedScorePopup(earnedScore);

        ModifyHiScore();
    }

    private void ModifyHiScore()
    {
        if (score >= highScore)
        {
            PlayerPrefs.SetInt("hiScore", score);
            highScore = score;
            hiScore.text = highScore.ToString();
        }
    }

    private void InstantiateAddedScorePopup(int earnedScore)
    {
        scorePopup = GameObject.Instantiate(pfab_txtScoreIncrease, PlayerCharacter.Instance.transform.position,
            Quaternion.identity, txtScore);

        Text foreground = scorePopup.GetChild(0).GetComponent<Text>();
        Text background = scorePopup.GetChild(1).GetComponent<Text>();

        foreground.text = "+" + earnedScore.ToString();
        background.text = "+" + earnedScore.ToString();

        scorePopup.GetComponent<AddedScoreText>().MovePopup();
    }

    public void AddMana(int amountOfManaToAdd)
    {
        currentMana += amountOfManaToAdd;
        
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        else if (currentMana < 0)
        {
            currentMana = 0;
        }

        float fillPercentage = (float) currentMana / (float) maxMana;
        manaBar.AdjustFillPercentage(fillPercentage);
        manaBar.AdjustInnerText(currentMana, maxMana);

        if (amountOfManaToAdd > 0)
        {
            AudioManager.Instance.PlayAddedManaClip(amountOfManaToAdd);
            manaPopup = GameObject.Instantiate(pfab_txtManaIncrease, manaBarTransform.position, Quaternion.identity,
                manaBarTransform);

            Text foreground = manaPopup.GetChild(0).GetComponent<Text>();
            Text background = manaPopup.GetChild(1).GetComponent<Text>();

            string sign = amountOfManaToAdd > 0 ? "+" : "-";


            foreground.text = sign + amountOfManaToAdd.ToString();
            background.text = sign + amountOfManaToAdd.ToString();

            manaPopup.GetComponent<AddedManaText>().MovePopup();
        }
        AdjustPowerupButtonVisibility();
    }

    public void StartUnlimitedBladeworks()
    {
        if (powerupInstance == null && currentMana >= bladeworksManaCost)
        {
            AddMana(-bladeworksManaCost);
            powerupInstance =
                GameObject.Instantiate(pfab_bladeworksPortal, new Vector3((Screen.width / 2), -325, 1),
                    Quaternion.identity); // 490 -360
        }
        else
        {
            AudioManager.Instance.PlayInvalidActionClip();
        }
    }

    public void StartMagmaCleaver()
    {
        if (powerupInstance == null && currentMana >= magmaCleaverManaCost)
        {
            AddMana(-magmaCleaverManaCost);
            powerupInstance =
                GameObject.Instantiate(pfab_magmaCleaver, new Vector3(-1000, 1290, 1), Quaternion.identity);
        }
        else
        {
            AudioManager.Instance.PlayInvalidActionClip();
        }
    }

    public void IncreasePlayerJumpCount()
    {
        if (currentMana >= jumpBoostManaCost && PlayerCharacter.Instance.GetMaxJumpCount() < maxBoostedPlayerJumpCount)
        {
            AddMana(-jumpBoostManaCost);
            jumpBoostPopup = GameObject.Instantiate(pfab_jumpBoostPopup, PlayerCharacter.Instance.transform.position,
                Quaternion.identity, txtScore);
            PlayerCharacter.Instance.IncreaseMaxJumpCount(1);

            Text foreground = jumpBoostPopup.GetChild(0).GetComponent<Text>();
            Text background = jumpBoostPopup.GetChild(1).GetComponent<Text>();

            foreground.text = "+1 Max Jump";
            background.text = "+1 Max Jump";

            AudioManager.Instance.PlayJumpBoostActivationClip();
        }
        else
        {
            AudioManager.Instance.PlayInvalidActionClip();
        }
    }

    public void TogglePauseMenu()
    {
        if (!isGamePaused)
        {
            ToggleFreezeGame();
            AudioManager.Instance.musicAudioSource.Stop();
            CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.Pause);
        }
        else
        {
            CanvasManager.Instance.DisableLastActivePanel();
            AudioManager.Instance.musicAudioSource.Play();
            ToggleFreezeGame();
        }
    }

    public void ToggleFreezeGame()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
            Time.timeScale = 1;
            
        }
        else
        {
            isGamePaused = true;
            Time.timeScale = 0;
        }
    }

    public void InitiateGameOverRoutine()
    {
        AudioManager.Instance.musicAudioSource.Stop();
        AudioManager.Instance.PlayDefeatClip();
        ToggleFreezeGame();
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.EndGame);
    }

    public void ResumeGameAndHealCastleAfterAd(int repairPercentage)
    {
        castle.RepairCastleAfterAd(repairPercentage);
        EnemyManager.Instance.DestroyAllEnemies();
        CanvasManager.Instance.DisableLastActivePanel();
        AudioManager.Instance.musicAudioSource.Play();
        isGamePaused = true;
        GameEventManager.Instance.ToggleFreezeGame();
    }

    public void ResumeGameAfterSkippingInterstitialAd()
    {
        CanvasManager.Instance.DisableLastActivePanel();
        AudioManager.Instance.musicAudioSource.Play();
        isGamePaused = false;
        Time.timeScale = 1;
    }

    public void OpenGameScene()
    {
        SceneManager.LoadScene(0); // First index is the SampleScene, the scene game actually takes place in
    }

    public void OpenMainMenuScene()
    {
        SceneManager.LoadScene(1); // Second index is the main menu
    }

    private void AdjustPowerupButtonVisibility()
    {
        ColorBlock bladeworkColors = bladeworksButton.colors;
        if (currentMana < bladeworksManaCost)
        {
            bladeworkColors.normalColor = notEnoughManaPowerupButtonColor; // Disabled color
        }
        else if (currentMana >= bladeworksManaCost)
        {
            bladeworkColors.normalColor = normalPowerupButtonColor; // Normal Color
        }
        
        bladeworksButton.colors = bladeworkColors;

        ColorBlock magmaCleaverColors = magmaCleaverButton.colors;
        if (currentMana < magmaCleaverManaCost)
        {
            magmaCleaverColors.normalColor = notEnoughManaPowerupButtonColor; // Disabled color
        }
        else if (currentMana >= magmaCleaverManaCost)
        {
            magmaCleaverColors.normalColor = normalPowerupButtonColor; // Normal color
        }

        magmaCleaverButton.colors = magmaCleaverColors;
    }

    public int GetScore()
    {
        return score;
    }
}