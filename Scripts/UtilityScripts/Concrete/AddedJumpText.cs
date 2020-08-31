using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddedJumpText : MonoBehaviour, IFloatingText
{
    public int speed;
    public float distanceToFloat;

    private float remainingDistanceBeforeDying;
    private float previousPosY;

    void Start()
    {
        transform.position = PlayerCharacter.Instance.transform.position;
        remainingDistanceBeforeDying = distanceToFloat;
    }

    void Update()
    {
        MovePopup();
    }


    public void MovePopup()
    {
        previousPosY = transform.position.y;
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        remainingDistanceBeforeDying -= Mathf.Abs(transform.position.y - previousPosY);

        if (remainingDistanceBeforeDying <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
