using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour, IBuilding
{
    public int MaxHealth, CurrentHealth;
    public IHealthBar healthBar;

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            GetDestroyed();
        }
    }

    public void GetDestroyed()
    {
        healthBar.MakeHealthBarInvisible();
        Debug.Log("Game over");
    }

    public void GetRepaired(int repairPercentage)
    {
        int healAmount = (MaxHealth / 100) * repairPercentage;
        CurrentHealth += healAmount;

        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    private void AdjustHealthBar()
    {
        int fillPercentage = (CurrentHealth / MaxHealth) * 100;
        healthBar.AdjustFillPercentage(fillPercentage);
    }
}
