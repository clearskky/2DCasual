using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeGameButton : MonoBehaviour, IButtonController
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        AudioManager.Instance.PlayButtonHitClip();
        GameEventManager.Instance.TogglePauseMenu();
    }
}
