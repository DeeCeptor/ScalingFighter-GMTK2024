using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player can take damage
/// </summary>
public class Player : ITriggerable
{
    public static Player Instance;
    void Awake()
    {
        Instance = this;
    }
}
