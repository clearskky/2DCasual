using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddedScoreText : MonoBehaviour, IFloatingText
{
    public int speed;
    public float distanceToFloat;

    private float remainingDistanceBeforeDying;
    private float previousPosY;

    void Awake()
    {
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y - distanceToFloat, transform.parent.position.z);
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
