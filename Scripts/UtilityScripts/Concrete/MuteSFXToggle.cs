using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MuteSFXToggle : MonoBehaviour, IToggleControl
{
    public AudioMixer sfxMixer;

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
            sfxMixer.SetFloat("sfxVol", -80.0f);
        }
        else
        {
            sfxMixer.SetFloat("sfxVol", 0.0f);
        }
    }
}
