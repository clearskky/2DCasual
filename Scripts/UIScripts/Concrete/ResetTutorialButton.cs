using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResetTutorialButton : MonoBehaviour, IButtonController
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
        PlayerPrefs.SetInt("tutorialHasBeenCompleted", 0);
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.MainMenu);
    }
}
