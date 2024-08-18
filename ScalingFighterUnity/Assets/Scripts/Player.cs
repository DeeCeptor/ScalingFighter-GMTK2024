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
    public ScalableObject Scaling;
    /// <summary>
    /// Gameobject tag on enemy colliders
    /// </summary>
    public string TagThatHurtsUs;

    public static Player Instance;
    void Awake()
    { 
        Instance = this;
        if (Movement == null) 
            Movement = this.GetComponent<MoveTowardsTarget>();
        FightManager.Instance.RegisterTarget(this.gameObject);
        // Add self to cinemachine target group
        AssetHolder.Instance.TargetGroup.AddMember(this.transform, 1f, 1f);
    }
    private void OnDestroy()
    {
        FightManager.Instance.RemoveTarget(this.gameObject);
    }

    public override void OnTriggered(GameObject entered, Vector3 position)
    {
        base.OnTriggered(entered, position);
        if (entered.CompareTag(TagThatHurtsUs))
        {
            // Lower communal player health
            Healthbar.Instance.TakeHit();
            GameObject obj = (GameObject)Instantiate(AssetHolder.Instance.DamageAnimation, position, Quaternion.identity);
        }
    }
    public override void OnCollision(GameObject collided, Vector3 position)
    {
        base.OnCollision(collided, position);
        if (collided.CompareTag(TagThatHurtsUs))
        {
            // Lower communal player health
            Healthbar.Instance.TakeHit();
            GameObject obj = (GameObject)Instantiate(AssetHolder.Instance.DamageAnimation, position, Quaternion.identity);
        }
    }
}
