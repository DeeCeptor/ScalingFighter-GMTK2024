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
    /// <summary>
    /// Periodically set Random float parameter to determine if we'll kick or punch
    /// </summary>
    public Animator Anims;

    public void SetTarget(Transform t)
    {
        this.Target = t;
    }


    GameObject GetClosestTarget(GameObject[] target)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in target)
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
            if (FightManager.Instance.TargetsPerTeam.ContainsKey(OpponentTagToTarget) &&
                FightManager.Instance.TargetsPerTeam[OpponentTagToTarget].Count > 0)
            {
                Target = GetClosestTarget(FightManager.Instance.TargetsPerTeam[OpponentTagToTarget].ToArray()).transform;
            }
        }
        if (Target == null)
            return;

        // Decide exactly WHERE we want to stand
        Vector3 destination = Target.transform.position;
        // We want to stand on EITHER left side or RIGHT of target, whichever's closest
        // Stand close or further based on our scale
        // PROBLEM: enemies also try to adjust their punching distance based on their size, so BOTH move. Maybe don't move if close enough to destination?
        // If smaller, need to stand CLOSER, not further away
        float actualDistanceWeWant = GetWithinTargetDist * Mathf.Abs(this.transform.localScale.x);
        Vector3 offset = new Vector3(actualDistanceWeWant, 0f, 0f);
        Vector3 leftSide = destination - offset;
        Vector3 rightSide = destination + offset;

        if (Vector3.Distance(this.transform.position, leftSide) < Vector3.Distance(this.transform.position, rightSide))
        {
            destination = leftSide;
        }
        else
            destination = rightSide;

        // Maybe backup if too close?
        if (Mathf.Abs(this.transform.position.x - destination.x) <= actualDistanceWeWant
            && Mathf.Abs(this.transform.position.z - destination.z) <= .01f)
        {
            // Stop moving, close enough
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

        if (Anims != null)
        {
            Anims.SetFloat("Random", Random.Range(0f, 1f));
        }

        //reset if they fall off the map
        if (this.transform.position.y < -20f)
        {
            this.transform.position = new Vector3(14.47f, 2f, 1f);
        }
    }
}
