using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Queues a bunch of animations. Punch, kick, etc
/// </summary>
public class FightManager : MonoBehaviour
{
    public Animator EnemyAnimator;
    public Animator PlayerAnimator;


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
