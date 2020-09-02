using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class InvertControlToggle : MonoBehaviour, IToggleControl
{
    private Toggle toggle;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate {
            OnToggleClick();
        });

        if (PlayerPrefs.GetInt("controlDirection") == 1)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }
    }



    public void OnToggleClick()
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt("controlDirection", -1);
        }
        else
        {
            PlayerPrefs.SetInt("controlDirection", 1);
        }
        PlayerPrefs.Save();
    }
}
