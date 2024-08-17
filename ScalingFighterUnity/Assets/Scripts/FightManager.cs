using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Queues a bunch of animations. Punch, kick, etc
/// </summary>
public class FightManager : MonoBehaviour
{
    /// <summary>
    /// <GameObjectTag, List<Damageable>
    /// </summary>
    public Dictionary<string, HashSet<Damageable>> TargetsPerTeam = new Dictionary<string, HashSet<Damageable>>();

    public Animator EnemyAnimator;
    public Animator PlayerAnimator;


    public void RegisterTarget(Damageable d)
    {
        if (!TargetsPerTeam.ContainsKey(d.tag))
            TargetsPerTeam[d.tag] = new HashSet<Damageable>();
        TargetsPerTeam[d.tag].Add(d);
    }
    public void RemoveTarget(Damageable d)
    {
        if (!TargetsPerTeam.ContainsKey(d.tag))
            return;
        TargetsPerTeam[d.tag].Remove(d);
    }

    public static FightManager Instance;
    void Awake()
    {
        Instance = this;
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(EnemyAnimator, "PunchLeft"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(PlayerAnimator, "PunchRight"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(EnemyAnimator, "PunchLeft"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(PlayerAnimator, "PunchRight"));
    }
}
