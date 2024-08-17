using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy moves towards player, sets their scale based on which side player is on so attacks punch right now
/// </summary>
public class Enemy : MonoBehaviour
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

    void Start()
    {
        Target = Player.Instance.transform;
    }


    void Update()
    {
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
