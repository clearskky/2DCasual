using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public float JumpForce, DefaultGravity, FallingGravity;
    public int raycastDistance;
    public int MaxJumpCount, CurrentJumpCount;

    [SerializeField] private bool isGrounded = true;
    private RaycastHit2D raycastHit2D;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        CurrentJumpCount = MaxJumpCount;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        StartCoroutine(CheckIfGrounded());
        AnimatePlayer();
    }

    // Rotate player to the enemy after colliding with it.
    public void RotateAfterHittingEnemy()
    {

    }

    // Start looking at the player's feet via raycast after a delay
    // Without this delay the player's velocity is made zero the instant the player jumps.
    IEnumerator CheckIfGrounded()
    {
        if (isGrounded == false)
        {
            yield return new WaitForSeconds(0.1f); // The delay is adjusted here
            Debug.DrawRay(transform.position, -Vector2.up * raycastDistance, Color.red);

            // The layer we want the raycast to collide with is the 12th layer so we create an appropriate layermask by performing a shifting operation.
            int bitmask = 1 << 12;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, raycastDistance, bitmask);

            if (hit.collider != null)
            {
                isGrounded = true;
                CurrentJumpCount = MaxJumpCount;
                rigidbody.velocity = Vector2.zero;
            }
        }
        
    }

    private void AnimatePlayer()
    {
        animator.SetFloat("verticalVelocity", rigidbody.velocity.y);
        animator.SetFloat("horizontalVelocity", rigidbody.velocity.x);

        // Flip the sprite vertically if the player is accelerating towards left
        if (rigidbody.velocity.x < -0.1)
        {
            spriteRenderer.flipX = true;
        }
        else if (rigidbody.velocity.x > 0.1)
        {
            spriteRenderer.flipX = false;
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
            rigidbody.AddForce(direction * JumpForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Can't jump.");
        }
        
    }
}
