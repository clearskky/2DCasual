using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour, IEnemy
{
    public int maxHealth, currentHealth;
    public int movementSpeed;
    public int attackDamage;
    public int scoreValue;

    private bool isAlive;
    private Animator animator;
    private int scrollDirection;
    private float remainingDistanceBeforeScrolling;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetBool("isAlive", true);
        isAlive = true;
        currentHealth = maxHealth;

        DetermineInitialScrollDirection();

        // Scroller can only travel in one direction for as much as the total width of the enemy spawn area
        remainingDistanceBeforeScrolling = EnemyManager.Instance.spawnAreaXAxisSemidiameter + Math.Abs(transform.position.x - EnemyManager.Instance.transform.position.x);
    }

    void FixedUpdate()
    {
        MoveScroller();
    }

    // If the scroller spawns on the left side of EnemyManager, start off by scrolling towards right and vice versa
    public void DetermineInitialScrollDirection()
    {
        if ((transform.position.x - transform.parent.position.x) > 0) 
        {
            scrollDirection = -1;
        }
        else
        {
            scrollDirection = 1;
        }
        FlipSpriteBasedOnScrollDirection(scrollDirection);
    }

    private void MoveScroller()
    {
        if (isAlive)
        {
            float previousPosX = transform.position.x;
            transform.Translate(Vector2.right * scrollDirection * movementSpeed * Time.deltaTime);
            remainingDistanceBeforeScrolling -= Math.Abs(transform.position.x - previousPosX);

            if (remainingDistanceBeforeScrolling <= 0)
            {
                transform.position = new Vector3(transform.position.x, (transform.position.y - 150), transform.position.z);
                scrollDirection *= -1;
                remainingDistanceBeforeScrolling = EnemyManager.Instance.spawnAreaXAxisSemidiameter * 2;
                FlipSpriteBasedOnScrollDirection(scrollDirection);
            }
        }
    }

    // A positive direction means the Scroller is going towards right and should also face right
    private void FlipSpriteBasedOnScrollDirection(int direction)
    {
        if (direction > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "castleWing")
        {
            IBuildingWing buildingWing = collision.gameObject.GetComponent<CastleWing>();
            buildingWing.RegisterDamage(attackDamage);

            InitiateDeathRoutine("castleWing");
        }
    }

    // This sets off the death animation
    public void InitiateDeathRoutine(string sourceOfDeathTag)
    {
        AudioManager.Instance.PlayScrollerDeathClip();

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

    // The animation event at the end of the death animation executes this method, killing the Scroller
    // We let the EnemyManager know of this tragedy so it can send reinforcements
    public void Die()
    {
        //PlayerCharacter.Instance.enemiesKilledBeforeFalling += 1;
        //GameEventManager.Instance.IncreaseScore(scoreValue * PlayerCharacter.Instance.enemiesKilledBeforeFalling);
        EnemyManager.Instance.DecrementCurrentScrollerCount();

        if (PlayerCharacter.Instance.targetEnemyTransform == this.transform)
        {
            PlayerCharacter.Instance.targetEnemyTransform = null;
            PlayerCharacter.Instance.isAttacking = false;
            PlayerCharacter.Instance.animator.SetBool("isAttacking", false);
        }

        Destroy(gameObject);
    }

    public void GetHealed(int healPercentage)
    {
        currentHealth += (maxHealth / 100) * healPercentage;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
