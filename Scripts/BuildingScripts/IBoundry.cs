using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoundry
{
    void OnCollisionEnter2D(Collision2D collision);
}
