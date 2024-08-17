using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Moves towards target, sets their scale based on which side player is on so attacks punch right now. Needs a different script to set target
/// </summary>
public class MoveTowardsTarget : MonoBehaviour
{
    /// <summary>
    /// Target we move towards and orient ourselves
    /// </summary>
    public Transform Target;
    /// <summary>
    /// How fast we move towards target
    /// </summary>
    public float MovementSpeed = 2f;
    /// <summary>
    /// How close we have to be to target to stop moving
    /// </summary>
    public float GetWithinTargetDist = 2f;
    /// <summary>
    /// Tag of enemy Damageable to move towards
    /// </summary>
    public string OpponentTagToTarget;

    public void SetTarget(Transform t)
    {
        this.Target = t;
    }


    Damageable GetClosestTarget(Damageable[] target)
    {
        Damageable tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Damageable t in target)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
    void Update()
    {
        if (Target == null)
        {
            // Try to find nearest opponent
            Target = GetClosestTarget(FightManager.Instance.TargetsPerTeam[OpponentTagToTarget].ToArray()).transform;
        }
        if (Target == null)
            return;
        // Maybe backup if too close?
        if (Vector2.Distance(this.transform.position, Target.transform.position) <= GetWithinTargetDist)
        {
            // Stop moving
        }
        else
        {
            // Move towards target
            float desiredXPos = Mathf.MoveTowards(this.transform.position.x, Target.transform.position.x, MovementSpeed * Time.deltaTime);
            Vector3 newPos = new Vector3(desiredXPos, this.transform.position.y, this.transform.position.z);
            this.transform.position = newPos;
            // Set X scale based on which side of target we're on
            Vector3 curLocalScale = this.transform.localScale;
            if (this.transform.position.x > Target.transform.position.x)
            {
                curLocalScale.x = -1f * Mathf.Abs(curLocalScale.x);
            }
            else
            {
                curLocalScale.x = 1f * Mathf.Abs(curLocalScale.x);
            }
            this.transform.localScale = curLocalScale;
        }
    }
}
