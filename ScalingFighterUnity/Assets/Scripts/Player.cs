using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player can take damage
/// </summary>
public class Player : ITriggerable
{
    /// <summary>
    /// Contains player health
    /// </summary>
    public Damageable PlayerDamage;

    public static Player Instance;
    void Awake()
    {
        Instance = this;
    }
}
