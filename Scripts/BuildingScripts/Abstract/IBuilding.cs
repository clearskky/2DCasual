using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBuilding
{
    void TakeDamage(int damage);
    void GetDestroyed();
    void GetRepaired(int repairPercentage);
}
