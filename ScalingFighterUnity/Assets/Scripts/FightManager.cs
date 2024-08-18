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

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    /// <summary>
    /// Spawn positions for new enemies/players
    /// </summary>
    public Transform SpawnPosLeft, SpawnPosRight;
    
    public Animator EnemyAnimator;
    public Animator PlayerAnimator;

    IEnumerator ProgressCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        yield return null;
        // INITIALLY, one player, one enemy

        // WAIT until enemies defeated
        while (TargetsPerTeam["Enemy"].Count > 0)
            yield return null;

        Debug.Log("Spawning two enemies");
        // Spawn enemy (left side)
        Instantiate(EnemyPrefab, SpawnPosLeft.position, Quaternion.identity);

        yield return new WaitForSeconds(10f);
        // Spawn enemy (right side)
        Instantiate(EnemyPrefab, SpawnPosRight.position, Quaternion.identity);

        while (TargetsPerTeam["Enemy"].Count > 0)
            yield return null;
        Debug.Log("Spawning friendly");
        // Spawn friendly
        Instantiate(PlayerPrefab, SpawnPosLeft.position, Quaternion.identity);
        // Wait
        yield return new WaitForSeconds(2f);
        // Spawn enemy
        Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(15f);
        // Spawn enemy (left side)
        Instantiate(EnemyPrefab, SpawnPosLeft.position, Quaternion.identity);
        // Spawn enemy (right side)
        Instantiate(EnemyPrefab, SpawnPosRight.position, Quaternion.identity);

        // Spawn enemy

        // Wait
        // Spawn enemy
        // Wait
        // Spawn enemy
        // Spawn friendly
        // Spawn enemy

        yield return null;

    }


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
        StartCoroutine(ProgressCoroutine());
        /*
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(EnemyAnimator, "PunchLeft"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(PlayerAnimator, "PunchRight"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(EnemyAnimator, "PunchLeft"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(PlayerAnimator, "PunchRight"));*/
    }
}
