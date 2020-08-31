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

            InitiateDeathRoutine("castleWing");
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            InitiateDeathRoutine(null);
        }
    }

    // This method triggers the death animation, which executes the Die() method at the end via an animation events
    public void InitiateDeathRoutine(string sourceOfDeathTag)
    {
        AudioManager.Instance.PlayFlyingEyeDeathClip();

        if (sourceOfDeathTag != "castleWing")
        {
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
