﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour, IManager
{
    private static CanvasManager _instance;
    public static CanvasManager Instance
    {
        get { return _instance; }
    }

    private List<PanelController> panelControllerList;
    private PanelController lastActivePanel;
    private string activeSceneName = "";

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

        panelControllerList = GetComponentsInChildren<PanelController>().ToList();
        panelControllerList.ForEach(panel => panel.gameObject.SetActive(false));
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            activeSceneName = "MainMenu";
            EnableSpecificPanel(TogglablePanelType.MainMenu);
        }
        else
        {
            activeSceneName = "SampleScene";
            lastActivePanel = null;
        }
    }

    void Update()
    {
        EnablePreviousPanelOnEscapeInput();
    }

    private void EnablePreviousPanelOnEscapeInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeSceneName == "SampleScene" && lastActivePanel == null)
            {
                AudioManager.Instance.PlayButtonHitClip();
                GameEventManager.Instance.TogglePauseMenu();
            }
            else if (lastActivePanel.previousPanelType == null)
            {
                Debug.LogWarning("Nowhere to go back to");
            }
            else if (lastActivePanel.previousPanelType == TogglablePanelType.none)
            {
                AudioManager.Instance.PlayButtonHitClip();
                GameEventManager.Instance.TogglePauseMenu();
            }
            else
            {
                AudioManager.Instance.PlayButtonHitClip();
                EnableSpecificPanel(lastActivePanel.previousPanelType);
            }
        }
    }

    public void EnableSpecificPanel(TogglablePanelType panelTypeToToggleOn)
    {
        panelControllerList.ForEach(panel => panel.gameObject.SetActive(false));

        PanelController panelToToggleOn =
                panelControllerList.Find(panel => panel.togglablePanelType == panelTypeToToggleOn);

        if (panelToToggleOn != null)
        { 
            panelToToggleOn.gameObject.SetActive(true); 
            lastActivePanel = panelToToggleOn;
        }
        else
        { 
            Debug.LogWarning("The Panel was not found!");
        }
    }

    public void DisableAllTogglablePanels()
    {
        panelControllerList.ForEach(panel => panel.gameObject.SetActive(false));
    }

    public void DisableLastActivePanel()
    {
        if (lastActivePanel != null)
        {
            lastActivePanel.gameObject.SetActive(false);
            lastActivePanel = null;
        }
        else
        {
            Debug.LogWarning("No panel has been activated yet");
        }
    }

    
}
