using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public float JumpForce, DefaultGravity, FallingGravity;
    public int MaxJumpCount, CurrentJumpCount;

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
        AnimatePlayer();
    }

    // Rotate player to the enemy after colliding with it.
    public void RotateAfterHittingEnemy()
    {

    }

    private void AnimatePlayer()
    {
        animator.SetFloat("verticalVelocity", rigidbody.velocity.y);
        animator.SetFloat("horizontalVelocity", rigidbody.velocity.x);

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
            CurrentJumpCount -= 1;
            rigidbody.AddForce(direction * JumpForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Can't jump.");
        }
        
    }
}
