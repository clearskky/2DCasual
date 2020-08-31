using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour, IHealthBar
{
    private Slider slider;
    [SerializeField] private Text manaText;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void AdjustFillPercentage(float fillPercentage)
    {
        slider.value = fillPercentage;
    }

    public void AdjustInnerText(int min, int max)
    {
        manaText.text = min.ToString() + "/" + max.ToString();
    }

    public void MakeHealthBarVisible()
    {
        throw new System.NotImplementedException();
    }

    public void MakeHealthBarInvisible()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator EaseIntoNewHealthValue(float fillPercentage)
    {
        throw new System.NotImplementedException();
    }
}
