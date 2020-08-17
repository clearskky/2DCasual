using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.WSA.Persistence;

public class PlayerCharacter : MonoBehaviour, IPlayer
{

    private static PlayerCharacter _instance;
    public static PlayerCharacter Instance {get {return _instance;}}

    public int JumpForce, BounceBackForce;
    public int raycastDistance;
    public int MaxJumpCount, CurrentJumpCount;
    public int AttackDamage;

    [SerializeField] private bool isGrounded = true;
    private RaycastHit2D _raycastHit2D;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        CurrentJumpCount = MaxJumpCount;
    }


    void Update()
    {
        StartCoroutine(CheckIfGrounded());
        AnimatePlayer();
    }

    // Rotate player to the enemy after colliding with it.
    public void RotateBeforeHittingEnemy(float enemyPosX)
    {
        if (transform.position.x < enemyPosX)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    // Start looking at the player's feet via raycast after a delay
    // Without this delay the player's velocity is made zero the instant the player jumps.
    public IEnumerator CheckIfGrounded()
    {
        if (isGrounded == false)
        {
            yield return new WaitForSeconds(0.1f); // The delay is adjusted here
            
            //Debug.DrawRay(transform.position, -Vector2.up * raycastDistance, Color.red);

            // The layer we want the raycast to collide with is the 12th layer so we create an appropriate layermask by performing a shifting operation.
            int bitmask = 1 << 12;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, raycastDistance, bitmask);

            if (hit.collider != null)
            {
                isGrounded = true;
                CurrentJumpCount = MaxJumpCount;
                _rigidbody.velocity = Vector2.zero; // We don't want the player to slide around after landing on solid ground
            }
        }
        
    }

    public void AnimatePlayer()
    {
        _animator.SetFloat("verticalVelocity", _rigidbody.velocity.y);
        _animator.SetFloat("horizontalVelocity", _rigidbody.velocity.x);

        // Flip the sprite vertically if the player is accelerating towards left
        if (_rigidbody.velocity.x < -0.1)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_rigidbody.velocity.x > 0.1)
        {
            _spriteRenderer.flipX = false;
        }
    }

    // This method is called by TouchManager.
    public void Jump(Vector2 direction)
    {
        if (CurrentJumpCount > 0)
        {
            Debug.Log("My people need me");
            isGrounded = false;
            CurrentJumpCount -= 1;
            _rigidbody.AddForce(direction * JumpForce, ForceMode2D.Impulse);
        }

    }

    public void StopAttacking()
    {
        _animator.SetBool("isAttacking", false);
    }

    public void StartAttacking()
    {
        _animator.SetBool("isAttacking", true);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        AttackCollidedEnemy(collision);
    }

    private void AttackCollidedEnemy(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            RotateBeforeHittingEnemy(collision.gameObject.transform.position.x);
            StartAttacking();
            StartCoroutine(BouncePlayerAwayFromEnemy(collision, 0.1f));

            IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();
            enemy.TakeDamage(AttackDamage);
        }
    }

    private IEnumerator BouncePlayerAwayFromEnemy(Collision2D collision, float delay)
    {
        yield return new WaitForSeconds(delay);
        _rigidbody.AddForce(collision.contacts[0].normal * BounceBackForce, ForceMode2D.Impulse);
    }
}
