using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    public Text hiScoreHeader;

    void Start()
    {
        hiScoreHeader.text = "your score was: " + GameEventManager.Instance.GetScore().ToString();
    }
}
