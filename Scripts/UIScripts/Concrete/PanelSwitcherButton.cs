using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PanelSwitcherButton : MonoBehaviour, IButtonController
{
    public TogglablePanelType panelTypeToSwitch;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        AudioManager.Instance.PlayButtonHitClip();
        CanvasManager.Instance.EnableSpecificPanel(panelTypeToSwitch);
    }
}
