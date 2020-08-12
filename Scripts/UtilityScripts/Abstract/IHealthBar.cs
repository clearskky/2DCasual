using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthBar
{
    // Sets the fill percentage of the health bar to the value of the fillPercentage
    void AdjustFillPercentage(int fillPercentage);
    void MakeHealthBarVisible();
    void MakeHealthBarInvisible();
}
