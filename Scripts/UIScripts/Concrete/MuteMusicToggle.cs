using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MuteMusicToggle : MonoBehaviour, IToggleControl
{
    public AudioMixer musicMixer;

    private Toggle toggle;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate {
            OnToggleClick();
        });

        if (PlayerPrefs.GetFloat("musicVol") <= -80.0f)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    public void OnToggleClick()
    {
        if (toggle.isOn)
        {
            musicMixer.SetFloat("musicVol", -80.0f);
            PlayerPrefs.SetFloat("musicVol", -80.0f);
        }
        else
        {
            musicMixer.SetFloat("musicVol", 0.0f);
            PlayerPrefs.SetFloat("musicVol", 0.0f);
        }
        PlayerPrefs.Save();
    }
}
