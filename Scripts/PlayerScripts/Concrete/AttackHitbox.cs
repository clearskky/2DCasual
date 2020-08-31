using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour, IAttackHitbox
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Transform enemyTransform = collision.gameObject.transform;
        PlayerCharacter.Instance.AssignTargetEnemy(enemyTransform);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Transform enemyTransform = collision.gameObject.transform;
        PlayerCharacter.Instance.AssignTargetEnemy(enemyTransform);
    }

    public void AttackAllEnemiesInRange()
    {
        throw new System.NotImplementedException();
    }
}
