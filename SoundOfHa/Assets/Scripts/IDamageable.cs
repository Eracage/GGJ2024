using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float m_CurrentHealth { get; set; }
    public void TakeDamage(float damage);
}
