using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour, IEnemy
{
    public int maxHealth, currentHealth;
    public int movementSpeed;
    public int attackDamage;
    public int scoreValue;

    [SerializeField] private Transform buildingWingContainer;
    private bool isAlive;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform targetBuildingWing;

    public DamageSource sourceOfLastTakenDamage;

    void Awake()
    {
        isAlive = true;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetBool("isAlive", true);
        targetBuildingWing = DetermineTargetWing();
    }

    void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "castleWing")
        {
            IBuildingWing buildingWing = collision.gameObject.GetComponent<CastleWing>();
            buildingWing.RegisterDamage(attackDamage);

            TakeDamage(maxHealth * 3, DamageSource.CWing);
        }
    }

    private Transform DetermineTargetWing()
    {
        int childIndex = Random.Range(0, buildingWingContainer.childCount);
        Transform targetBuildingWing = buildingWingContainer.GetChild(childIndex);

        RotateSpriteBasedOnTargetWing(targetBuildingWing);

        return buildingWingContainer.GetChild(childIndex);
    }

    private void RotateSpriteBasedOnTargetWing(Transform targetWingTransform)
    {
        if (targetWingTransform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }

    // Moves the FlyingEye towards the designated CastleWing
    public void MoveTowardsTarget()
    {
        if (isAlive)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetBuildingWing.position,
                movementSpeed * Time.deltaTime
            );
        }

    }

    public void TakeDamage(int damage, DamageSource damageSource)
    {
        currentHealth -= damage;
        sourceOfLastTakenDamage = damageSource;
        if (currentHealth <= 0)
        {
            InitiateDeathRoutine();
        }
    }

    // This method triggers the death animation, which executes the Die() method at the end via an animation events
    public void InitiateDeathRoutine()
    {
        if (sourceOfLastTakenDamage == DamageSource.CWing)
        {
            AudioManager.Instance.PlayFlyingEyeDeathClip();
            
        }
        else if (sourceOfLastTakenDamage == DamageSource.Powerup)
        {
            AudioManager.Instance.PlayFlyingEyeDeathClip();
            GameEventManager.Instance.IncreaseScore(scoreValue);
        }
        else if (sourceOfLastTakenDamage == DamageSource.Player)
        {
            AudioManager.Instance.PlayIzanagiSwordClip();
            PlayerCharacter.Instance.enemiesKilledBeforeFalling += 1;
            GameEventManager.Instance.IncreaseScore(scoreValue * PlayerCharacter.Instance.enemiesKilledBeforeFalling);
        }

        isAlive = false;
        animator.SetBool("isAlive", false);
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
    }

    // The animation event at the end of the death animation executes this method, killing the FlyingEye
    // We let the EnemyManager know of this tragedy so it can send reinforcements
    public void Die()
    {
        EnemyManager.Instance.DecrementCurrentFlyingEyeCount();

        if (PlayerCharacter.Instance.targetEnemyTransform == this.transform)
        {
            PlayerCharacter.Instance.targetEnemyTransform = null;
            PlayerCharacter.Instance.isAttacking = false;
            PlayerCharacter.Instance.animator.SetBool("isAttacking", false);
        }

        Destroy(gameObject);
    }

    // Heals this unit by a percentage of its max health
    public void GetHealed(int healPercentage)
    {
        currentHealth += (maxHealth / 100) * healPercentage;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
