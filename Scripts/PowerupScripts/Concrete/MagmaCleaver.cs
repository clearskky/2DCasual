using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaCleaver : MonoBehaviour, IPowerupProjectile
{
    public int attackDamage;
    public int rotationRate;

    public int secondsUntilSelfDestruct;

    private Quaternion targetRotation;
    private float selfDestructTime;

    void Start()
    {
        targetRotation = Quaternion.Euler(0, 0, -180);
        selfDestructTime = Time.time + secondsUntilSelfDestruct;
        AudioManager.Instance.PlayMagmaCleaverActivationClip();
    }

    void Update()
    {
        SliceUp();
    }

    private void SliceUp()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationRate * Time.deltaTime);

        if (Time.time >= selfDestructTime)
        {
            Die();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();
        enemy.TakeDamage(attackDamage);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
