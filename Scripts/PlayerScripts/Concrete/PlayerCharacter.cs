using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

//using UnityEngine.XR.WSA.Persistence;

public class PlayerCharacter : MonoBehaviour, IPlayer
{

    private static PlayerCharacter _instance;
    public static PlayerCharacter Instance {get {return _instance;}}

    public float jumpForce, bounceBackForce;
    public int raycastDistance;
    public int maxJumpCount, currentJumpCount;

    [Range(1.0f, 2.0f)] // 1 equates to the base jumpforce
    public float midAirJumpForceMultiplier; // Midair jumpforce = jumpforce * (1 / midAirJumpForceMultiplier)

    public int attackDamage;
    public int enemiesKilledBeforeFalling;

    [SerializeField] private bool isGrounded = true;
    private RaycastHit2D raycastHit2D;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    

    public Animator animator;
    public Transform targetEnemyTransform;
    public bool isAttacking;
    

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("isAttacking", false);

        currentJumpCount = maxJumpCount;
        isAttacking = false;
        targetEnemyTransform = null;
        enemiesKilledBeforeFalling = 0;
    }

    void Update()
    {
        StartCoroutine(CheckIfGrounded());
        AnimatePlayer();
    }

    // This method is called by the AttackHitbox of the player.
    public void AssignTargetEnemy(Transform enemy)
    {
        if (CanATargetBeAssigned())
        {
            targetEnemyTransform = enemy;
            //Debug.Log("I have acquired a new target");
            StartAttacking();
        }
    }

    public bool CanATargetBeAssigned()
    {
        if (targetEnemyTransform != null || isAttacking || animator.GetBool("isAttacking") || animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack"))
        {
            //string errorLog = "New target cannot be assigned: ";
            //if (targetEnemyTransform != null)
            //{
            //    errorLog += "I already have a target";
            //}
            //if (isAttacking == true)
            //{
            //    errorLog += ", I'm already attacking";
            //}
            //if (animator.GetBool("isAttacking"))
            //{
            //    errorLog += ", Animator is still attacking";
            //}
            //Debug.Log(errorLog);
            return false;
        }
        else
        {
            return true;
        }
    }

    public void StartAttacking()
    {
        if (isAttacking == false && animator.GetBool("isAttacking") == false)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            //animator.SetBool("isAttacking", true);
        }
    }

    public void AttackTargetEnemy()
    {
        if (targetEnemyTransform != null)
        {
            //Debug.Log("Attack Animation Started");
            IEnemy enemy = targetEnemyTransform.GetComponent<IEnemy>();
            DamageSpecifiedEnemy(enemy);
            StartCoroutine(GameEventManager.Instance.FreezeTime(0.05f));
        }
    }

    private void DamageSpecifiedEnemy(IEnemy enemy)
    {
        BouncePlayerAwayFromTargetEnemy();
        enemy.TakeDamage(attackDamage);
        isAttacking = false;
        targetEnemyTransform = null;
        //Debug.Log("Target Enemy has been damaged");
    }

    public void StopAttacking()
    {
        //animator.ResetTrigger("Attack");
        //animator.SetBool("isAttacking", false);
        
        //Debug.Log("Attack animation stopped. Target enemy status: " + targetEnemyTransform + " I have disposed of him ehehe");
    }

    public void BouncePlayerAwayFromTargetEnemy()
    {
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);

        Vector2 bounceDirection = new Vector2(transform.position.x, transform.position.y) -
                                  new Vector2(targetEnemyTransform.position.x, targetEnemyTransform.position.y);

        rigidbody.AddForce(bounceDirection * bounceBackForce, ForceMode2D.Impulse);
    }

    // Start looking at the player's feet via raycast after a delay
    // Without this delay the player's velocity is made zero the instant the player jumps.
    public IEnumerator CheckIfGrounded()
    {
        if (isGrounded == false)
        {
            yield return new WaitForSeconds(0.1f); // The delay is adjusted here

            // The layer we want the raycast to collide with is the 12th layer so we create an appropriate layermask by performing a shifting operation.
            int bitmask = 1 << 12;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, raycastDistance, bitmask);

            if (hit.collider != null)
            {
                isGrounded = true;
                isAttacking = false;
                targetEnemyTransform = null;
                animator.SetBool("isAttacking", false);
                GameEventManager.Instance.AddMana(enemiesKilledBeforeFalling);
                enemiesKilledBeforeFalling = 0;
                currentJumpCount = maxJumpCount;
                rigidbody.velocity = Vector2.zero; // We don't want the player to slide around after landing on solid ground
            }
        }

    }

    // This method is called by TouchManager.
    public void Jump(Vector2 direction)
    {
        if (currentJumpCount > 0)
        {
            isGrounded = false;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);

            if (currentJumpCount != maxJumpCount)
            {
                rigidbody.AddForce(direction * (jumpForce * (1 / midAirJumpForceMultiplier)), ForceMode2D.Impulse); // If the player is jumping mid-air they can only jump with 1/3rd of the usual force
            }
            else
            {
                rigidbody.AddForce(direction * jumpForce, ForceMode2D.Impulse);
            }
            
            currentJumpCount -= 1;
            AudioManager.Instance.PlayPlayerJumpClip();
        }
    }

    public void AnimatePlayer()
    {
        animator.SetFloat("verticalVelocity", rigidbody.velocity.y);
        animator.SetFloat("horizontalVelocity", rigidbody.velocity.x);

        FlipPlayerSpriteHorizontally();
    }

    // Flip the sprite vertically if the player is accelerating towards left and vice versa
    private void FlipPlayerSpriteHorizontally()
    {
        if (!isAttacking)
        {
            if (rigidbody.velocity.x < -0.1)
            {
                spriteRenderer.flipX = true;
            }
            else if (rigidbody.velocity.x > 0.1)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    public void RotateBeforeHittingEnemy(float enemyPosX)
    {
        if (transform.position.x < enemyPosX)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true; //Makes the player character turn left since he is facing right by default.
        }
    }

    public IEnumerator BouncePlayerAwayFromPoint(Vector2 contactPoint, float delay)
    {
        yield return new WaitForSeconds(delay);
        rigidbody.AddForce(contactPoint * bounceBackForce, ForceMode2D.Impulse);
    }

    public Transform TellTheTargetEnemy()
    {
        return targetEnemyTransform;
    }

    private void AttackCollidedEnemy(Collision2D collision)
    {
        Debug.Log("Attacking: " + isAttacking.ToString() + " my target is " + targetEnemyTransform);
        if (collision.gameObject.tag == "enemy" && isAttacking == false)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            targetEnemyTransform = collision.gameObject.transform;
            IEnemy enemy = targetEnemyTransform.GetComponent<IEnemy>();
            StartCoroutine(GameEventManager.Instance.FreezeTime(0.05f));
            DamageSpecifiedEnemy(enemy);
        }
    }

    public bool DoesPlayerHaveATarget()
    {
        if (targetEnemyTransform != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetMaxJumpCount()
    {
        return maxJumpCount;
    }

    public void IncreaseMaxJumpCount(int increaseAmount)
    {
        maxJumpCount += increaseAmount;
        currentJumpCount += increaseAmount;
    }
}
