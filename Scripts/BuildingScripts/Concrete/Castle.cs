using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour, IBuilding
{
    public int MaxHealth, CurrentHealth;
    public Transform healthBar;
    private IHealthBar hpBar;
    void Start()
    {
        CurrentHealth = MaxHealth;
        hpBar = healthBar.GetComponent<IHealthBar>();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        float healthPercentage = ((float)CurrentHealth / (float)MaxHealth);
        //Debug.Log("Fill percentage is: " + healthPercentage.ToString());

        if (CurrentHealth <= 0)
        {
            GetDestroyed();
        }
        else
        {
            //StartCoroutine(hpBar.EaseIntoNewHealthValue(healthPercentage));
            hpBar.AdjustFillPercentage(healthPercentage);
        }
    }

    public void GetDestroyed()
    {
        //Debug.Log("Game over");
        //hpBar.MakeHealthBarInvisible();
        GameEventManager.Instance.InitiateGameOverRoutine();
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
        hpBar.AdjustFillPercentage(fillPercentage);
    }
}
