using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The castle has three different hitboxes, certain enemies will choose one of the three hitboxes at random to be their attack target.
// Damage taken from different hitboxes is subtracted from the total HP of the castle, game ends once castle's HP is depleted.
public class CastleWing : MonoBehaviour, IBuildingWing
{
    public Castle castle;

    // The damage taken by the castle wing is forwarded to the castle to handle the associated events
    // Such as subtracting the damage from the health bar and checking if the game should be over or not.
    public void RegisterDamage(int damage)
    {
        castle.TakeDamage(damage);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        throw new System.NotImplementedException();
    }
}
