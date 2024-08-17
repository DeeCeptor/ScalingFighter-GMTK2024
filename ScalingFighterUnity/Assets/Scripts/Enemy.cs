using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets movement target to player
/// </summary>
public class Enemy : MonoBehaviour
{
    public MoveTowardsTarget Movement;

    void Start()
    {
        if (Movement == null)
            this.Movement = this.GetComponent<MoveTowardsTarget>();
        Movement.SetTarget(Player.Instance.transform);
    }
}
