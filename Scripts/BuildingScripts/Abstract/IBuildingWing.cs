using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBuildingWing
{
    void RegisterDamage(int damage);

    void OnCollisionEnter2D(Collision2D collision);
}
