using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayWithTutorialButton : MonoBehaviour
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
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("tutorialHasBeenCompleted", 0) == 1)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.HowToPlay);
        }
        
    }
}
