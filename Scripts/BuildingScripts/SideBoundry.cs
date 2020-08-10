using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SideBoundry : MonoBehaviour, IBoundry
{
    public PlayerCharacter Player;
    public float PushForce;
    private Vector2 PushDirection;

    

    public void OnCollisionEnter2D(Collision2D collision)
    {
        PushDirection = new Vector2(collision.contacts[0].point.x - Player.transform.position.x, 0);
        PushDirection = (-1) * PushDirection.normalized * PushForce;

        Rigidbody2D playerRB = Player.GetComponent<Rigidbody2D>();
        playerRB.AddForce(PushDirection * PushForce, ForceMode2D.Impulse);
        Debug.Log("Back off");
    }
}
