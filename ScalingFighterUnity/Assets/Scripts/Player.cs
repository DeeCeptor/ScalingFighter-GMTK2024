using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Player can take damage, moves towards nearest enemy
/// </summary>
public class Player : ITriggerable
{
    public MoveTowardsTarget Movement;
    /// <summary>
    /// Contains player health
    /// </summary>
    public Damageable PlayerDamage;

    public static Player Instance;
    void Awake()
    {
        Instance = this;
        if (Movement == null)
            Movement = this.GetComponent<MoveTowardsTarget>();
        // Add self to cinemachine target group
        AssetHolder.Instance.TargetGroup.AddMember(this.transform, 1f, 1f);
    }
}
