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
    }

    public void OnToggleClick()
    {
        if (toggle.isOn)
        {
            musicMixer.SetFloat("musicVol", -80);
        }
        else
        {
            musicMixer.SetFloat("musicVol", 0);
        }
    }
}
