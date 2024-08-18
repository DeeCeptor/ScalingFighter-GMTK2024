using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    /// Spawn boundaries for new units
    /// </summary>
    public Transform SpawnPosTopLeft, SpawnPosBottomRight;
    
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
        Instantiate(EnemyPrefab, SpawnPosTopLeft.position, Quaternion.identity);

        yield return new WaitForSeconds(10f);
        // Spawn enemy (right side)
        Instantiate(EnemyPrefab, SpawnPosBottomRight.position, Quaternion.identity);

        while (TargetsPerTeam["Enemy"].Count > 0)
            yield return null;
        Debug.Log("Spawning friendly");
        // Spawn friendly
        Instantiate(PlayerPrefab, GetRandomSpawnPos(), Quaternion.identity);
        // Wait
        yield return new WaitForSeconds(2f);
        // Spawn enemy
        Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(15f);
        // Spawn enemy (left side)
        Instantiate(EnemyPrefab, GetRandomSpawnPos(), Quaternion.identity);
        // Spawn enemy (right side)
        Instantiate(EnemyPrefab, GetRandomSpawnPos(), Quaternion.identity);

        // Spawn enemies indefinitely, faster and faster
        float minSpawnTime = 10f;   // In seconds
        float maxSpawnTime = 30f;
        int loopNumber = 0;
        while (true)
        {
            Instantiate(EnemyPrefab, GetRandomSpawnPos(), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            Instantiate(EnemyPrefab, GetRandomSpawnPos(), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            Instantiate(EnemyPrefab, GetRandomSpawnPos(), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            Instantiate(EnemyPrefab, GetRandomSpawnPos(), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            // Give player new friendly if count is low enough
            if (TargetsPerTeam["Player"].Count < 3)
                Instantiate(PlayerPrefab, GetRandomSpawnPos(), Quaternion.identity);

            // Make them spawn faster
            minSpawnTime = Mathf.Max(5f, minSpawnTime - 1f);
            maxSpawnTime = Mathf.Max(minSpawnTime, maxSpawnTime - 2f);
            loopNumber++;
        }

        yield return null;

    }

    public bool GameIsOver = false;
    /// <summary>
    /// SetActive if player loses
    /// </summary>
    public GameObject GameOverPanel;
    IEnumerator GameOverChecker()
    {
        yield return new WaitForSeconds(0.5f);
        yield return null;
        while (true)
        {
            yield return null;
            if (!GameIsOver && TargetsPerTeam["Player"].Count <= 0)
            {
                Debug.Log("Game over!");
                GameIsOver = true;
                Time.timeScale = 0f;
                GameOverPanel.SetActive(true);
            }
        }
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public Vector3 GetRandomSpawnPos()
    {
        return new Vector3(
            Random.Range(SpawnPosTopLeft.position.x, SpawnPosBottomRight.position.x)
            , Random.Range(SpawnPosTopLeft.position.y, SpawnPosBottomRight.position.y) + 10f    // Drop in from above!
            , Random.Range(SpawnPosTopLeft.position.z, SpawnPosBottomRight.position.z));
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

    private void Update()
    {
        // User STOPPED fast-forwarding, restore normal timescale speed
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Time.timeScale = 1f;

        }
        // FAST FORWARD if player holds TAB
        else if (Input.GetKey(KeyCode.Tab))
        {
            Time.timeScale = 4f;
        }
    }

    public static FightManager Instance;
    void Awake()
    {
        Instance = this;
        StartCoroutine(ProgressCoroutine());
        StartCoroutine(GameOverChecker());
        /*
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(EnemyAnimator, "PunchLeft"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(PlayerAnimator, "PunchRight"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(EnemyAnimator, "PunchLeft"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(PlayerAnimator, "PunchRight"));*/
    }
}
