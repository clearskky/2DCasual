﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour, IManager
{
    public AudioSource sfxAudioSource, musicAudioSource;
    public AudioMixer sfxAudioMixer, musicAudioMixer;

    [Header("Player Clips")]
    public AudioClip jumpClip, playerBoundryBounceBackClip, addedManaClip;

    [Header("Enemy Clips")]
    public AudioClip scrollerDeathClip, flyingEyeDeathClip;

    [Header("Powerup Clips")]
    public AudioClip bladeworksActivationClip, magmaCleaverActivationClip, jumpBoostActivationClip;

    [Header("UI Clips")]
    public AudioClip mainMenuMusicClip, gameplayMusicClip, invalidActionClip, defeatClip, buttonHitClip;

    // Singleton Implementation
    private static AudioManager _instance;
    public static AudioManager Instance {get {return _instance;}}

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        musicAudioMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
        sfxAudioMixer.SetFloat("sfxVol", PlayerPrefs.GetFloat("sfxVol"));

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            musicAudioSource.clip = mainMenuMusicClip;
            musicAudioSource.Play();
            musicAudioSource.loop = true;
        }
        else
        {
            musicAudioSource.clip = gameplayMusicClip;
            musicAudioSource.Play();
            musicAudioSource.loop = true;
        }
    }

    public void PlayPlayerJumpClip()
    {
        sfxAudioSource.PlayOneShot(jumpClip);
    }

    public void PlayPlayerBoundryBounceBackClip()
    {
        sfxAudioSource.PlayOneShot(playerBoundryBounceBackClip);
    }

    public void PlayAddedManaClip()
    {
        sfxAudioSource.PlayOneShot(addedManaClip);
    }

    public void PlayScrollerDeathClip()
    {
        sfxAudioSource.PlayOneShot(scrollerDeathClip);
    }

    public void PlayFlyingEyeDeathClip()
    {
        sfxAudioSource.PlayOneShot(flyingEyeDeathClip);
    }

    public void PlayBladeworksActivationClip()
    {
        sfxAudioSource.PlayOneShot(bladeworksActivationClip);
    }

    public void PlayMagmaCleaverActivationClip()
    {
        sfxAudioSource.PlayOneShot(magmaCleaverActivationClip);
    }

    public void PlayJumpBoostActivationClip()
    {
        sfxAudioSource.PlayOneShot(jumpBoostActivationClip);
    }

    public void PlayInvalidActionClip()
    {
        sfxAudioSource.PlayOneShot(invalidActionClip);
    }

    public void PlayDefeatClip()
    {
        sfxAudioSource.PlayOneShot(defeatClip);
    }

    public void PlayButtonHitClip()
    {
        sfxAudioSource.PlayOneShot(buttonHitClip);
    }
}
