using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBlade : MonoBehaviour, IPowerupProjectile
{
    public int attackDamage;
    public int movementSpeed;

    private float remainingDistanceBeforeDying, previousPosY;

    void Start()
    {
        remainingDistanceBeforeDying = movementSpeed;
        AudioManager.Instance.PlayBladeworksActivationClip();
    }

    void Update()
    {
        TravelUpward();
    }

    void TravelUpward()
    {
        previousPosY = transform.position.y;
        transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
        remainingDistanceBeforeDying -= Mathf.Abs(transform.position.y - previousPosY);

        if (remainingDistanceBeforeDying <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();
        enemy.TakeDamage(attackDamage);
    }
}
