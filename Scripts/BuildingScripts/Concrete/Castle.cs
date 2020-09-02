using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour, IBuilding
{
    public int maxHealth, currentHealth;
    
    public Transform healthBar;
    private IHealthBar hpBar;

    private float healthPercentage;

    [SerializeField] private Image castleImg;
    [SerializeField] private Sprite dmgLvl1;
    [SerializeField] private Sprite dmgLvl2;
    [SerializeField] private Sprite dmgLvl3;
    [SerializeField] private Sprite dmgLvl4;
    [SerializeField] private Sprite dmgLvl5;
    private Sprite currentSprite;
    void Start()
    {
        castleImg.sprite = dmgLvl1;
        currentSprite = dmgLvl1;
        currentHealth = maxHealth;
        hpBar = healthBar.GetComponent<IHealthBar>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthPercentage = ((float)currentHealth / (float)maxHealth) * 100;

        if (currentHealth <= 0)
        {
            GetDestroyed();
        }
        else
        {
            ChangeDamageLevel(healthPercentage);
            AdjustHealthBar(healthPercentage);
        }
    }

    private void ChangeDamageLevel(float healthPercentage)
    {
        if (healthPercentage <= 20.0f)
        {
            castleImg.sprite = dmgLvl5;
        }
        else if (healthPercentage <= 40.0f)
        {
            castleImg.sprite = dmgLvl4;
        }
        else if (healthPercentage <= 60.0f)
        {
            castleImg.sprite = dmgLvl3;
        }
        else if (healthPercentage <= 80.0f)
        {
            castleImg.sprite = dmgLvl2;
        }
        else
        {
            castleImg.sprite = dmgLvl1;
        }
    }

    public void GetDestroyed()
    {
        GameEventManager.Instance.InitiateGameOverRoutine();
    }

    public void GetRepaired(int repairPercentage)
    {
        int healAmount = (maxHealth / 100) * repairPercentage;
        currentHealth += healAmount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthPercentage = ((float)currentHealth / (float)maxHealth) * 100;

        ChangeDamageLevel(healthPercentage);
        AdjustHealthBar(healthPercentage);
    }

    private void AdjustHealthBar(float healthPercentage)
    {
        hpBar.AdjustFillPercentage(healthPercentage);
    }
}
