using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPowerupProjectile
{
    void OnTriggerEnter2D(Collider2D collision);
}
