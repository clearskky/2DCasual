using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float JumpForce;
    public float MaxHorizontalVelocity;
    public float MaxVerticalVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatePlayer();
    }

    private void AnimatePlayer()
    {
        //Vector2 characterScale = transform.localScale;
        //if (rb.velocity.x < 0)
        //{
        //    characterScale.x = -1;
        //}
        //if (rb.velocity.x > 0)
        //{
        //    characterScale.x = 1;
        //}
        //transform.localScale = characterScale;

        animator.SetFloat("verticalVelocity", rb.velocity.y);
        animator.SetFloat("horizontalVelocity", rb.velocity.x);
    }

    public void Jump(Vector2 direction)
    {
        rb.AddForce(direction * JumpForce, ForceMode2D.Impulse);
    }

    private Vector2 NormalizeJumpVelocity(Vector2 velocity)
    {
        if (velocity.x > MaxHorizontalVelocity)
        {
            velocity.x = MaxHorizontalVelocity;
        }
        else if (velocity.x < (MaxHorizontalVelocity * (-1)))
        {
            velocity.x = (MaxHorizontalVelocity * (-1));
        }

        if (velocity.y > MaxVerticalVelocity)
        {
            velocity.y = MaxVerticalVelocity;
        }
        else if (velocity.y < (MaxVerticalVelocity * (-1)))
        {
            velocity.y = (MaxVerticalVelocity * (-1));
        }
        Debug.Log(velocity);
        return velocity;
    }
}
