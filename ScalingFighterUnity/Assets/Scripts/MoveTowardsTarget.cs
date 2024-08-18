using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

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
            if (FightManager.Instance.TargetsPerTeam[OpponentTagToTarget].Count > 0)
            {
                Target = GetClosestTarget(FightManager.Instance.TargetsPerTeam[OpponentTagToTarget].ToArray()).transform;
            }
        }
        if (Target == null)
            return;

        Vector3 destination = Target.transform.position;
        // We want to be EITHER on left side or RIGHT of target, whichever's closest
        Vector3 leftSide = destination + new Vector3(-GetWithinTargetDist, 0f, 0f);
        Vector3 rightSide = destination + new Vector3(GetWithinTargetDist, 0f, 0f);
        
        if (Vector3.Distance(this.transform.position, leftSide) < Vector3.Distance(this.transform.position, rightSide))
        {
            destination = leftSide;
        }
        else
            destination = rightSide;

        // Maybe backup if too close?
        if (Mathf.Abs(this.transform.position.x - destination.x) <= GetWithinTargetDist
            && Mathf.Abs(this.transform.position.z - destination.z) <= .01f)
        {
            // Stop moving
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, destination, MovementSpeed * Time.deltaTime);
        }
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
