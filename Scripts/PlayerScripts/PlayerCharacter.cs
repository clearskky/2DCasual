using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public float JumpForce, DefaultGravity, FallingGravity;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatePlayer();
        ModifyGravity();
    }

    private void ModifyGravity()
    {

    }

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

    public void Jump(Vector2 direction)
    {
        Debug.Log("My people need me");
        rigidbody.AddForce(direction * JumpForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {

    }
}
