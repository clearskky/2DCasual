using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBoundry : MonoBehaviour, IBoundry
{
    public Rigidbody2D playerRB;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        playerRB.velocity = Vector2.zero;
    }
}
