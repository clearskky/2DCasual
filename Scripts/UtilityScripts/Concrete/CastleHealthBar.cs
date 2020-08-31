using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthBar : MonoBehaviour, IHealthBar
{
    public float easeTime;
    private Slider slider;

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
        throw new System.NotImplementedException();
    }

    public IEnumerator EaseIntoNewHealthValue(float fillPercentage)
    {
        
        slider.value = Mathf.Lerp(slider.value, fillPercentage, easeTime);
        
        yield return new WaitForSeconds(easeTime);

    }

    public void MakeHealthBarVisible()
    {
        throw new System.NotImplementedException();
    }

    public void MakeHealthBarInvisible()
    {
        throw new System.NotImplementedException();
    }
}
