﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeDamage(int damage);

    void InitiateDeathRoutine();
    void Die();
    void GetHealed(int healPercentage);
}