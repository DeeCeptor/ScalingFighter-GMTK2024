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
    /// Defeating an enemy gives 1 score
    /// </summary>
    public int Score = 0;
    /// <summary>
    /// Defeating one enemy gives this much score
    /// </summary>
    public const int ScorePerEnemy = 100;

    public void AddToScore(int amount)
    {
        Score += amount;
        // Update visible score text
        AssetHolder.Instance.ScoreText.text = "" + Score;
    }

    /// <summary>
    /// <GameObjectTag, List<Damageable>
    /// </summary>
    public Dictionary<string, HashSet<GameObject>> TargetsPerTeam = new Dictionary<string, HashSet<GameObject>>();

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    /// <summary>
    /// Spawn boundaries for new units
    /// </summary>
    public Transform SpawnPosTopLeft, SpawnPosBottomRight;


    IEnumerator TutorialCo()
    {
        yield return null;
        // Start conversation
        // Wait until player has shrunk once
        while (!Player.Instance.Scaling.HasBeenRightClicked)
            yield return null;
        VNSceneManager.scene_manager.Button_Pressed();
        // Wait until player has grown once
        while (!Player.Instance.Scaling.HasBeenLeftClicked)
            yield return null;
        VNSceneManager.scene_manager.Button_Pressed();
    }

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
        SpawnPrefab(EnemyPrefab, true, specificPos: SpawnPosTopLeft.position);

        yield return new WaitForSeconds(10f);
        // Spawn enemy (right side)
        SpawnPrefab(EnemyPrefab, false, specificPos: SpawnPosBottomRight.position);

        while (TargetsPerTeam["Enemy"].Count > 0)
            yield return null;
        Debug.Log("Spawning friendly");
        // Spawn friendly
        SpawnPrefab(PlayerPrefab, false);

        // Wait
        yield return new WaitForSeconds(2f);
        // Spawn enemy
        SpawnPrefab(EnemyPrefab, false, specificPos:new Vector3(0, 10f, 0f));
        yield return new WaitForSeconds(15f);
        // Spawn enemy (left side)
        SpawnPrefab(EnemyPrefab, false);
        SpawnPrefab(EnemyPrefab, false);

        // Spawn enemies indefinitely, faster and faster
        float minSpawnTime = 10f;   // In seconds
        float maxSpawnTime = 20f;
        int loopNumber = 0;
        while (true)
        {
            SpawnPrefab(EnemyPrefab, true);
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            SpawnPrefab(EnemyPrefab, true);
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            SpawnPrefab(EnemyPrefab, true);
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            SpawnPrefab(EnemyPrefab, true);
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            // Give player new friendly if count is low enough
            if (TargetsPerTeam["Player"].Count < 3)
                SpawnPrefab(PlayerPrefab, false);

            // Make them spawn faster
            minSpawnTime = Mathf.Max(5f, minSpawnTime - 1f);
            maxSpawnTime = Mathf.Max(minSpawnTime, maxSpawnTime - 2f);
            loopNumber++;
        }

        yield return null;

    }
    /// <summary>
    /// Spawns given prefab, with optional random scale. If no specificPos given, random position is used
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="randomScale"></param>
    /// <param name="specificPos"></param>
    public GameObject SpawnPrefab(GameObject prefab, bool randomScale, Vector3 specificPos=default(Vector3))
    {
        Vector3 spawnPos = specificPos;
        if (spawnPos == default(Vector3))
            spawnPos = GetRandomSpawnPos();
        GameObject newObj = (GameObject)Instantiate(prefab, spawnPos, Quaternion.identity);
        if (randomScale)
        {
            float newScale = Random.Range(0.4f, 4f);
            newObj.transform.localScale = Vector3.one * newScale;
        }
        return newObj;
    }

    public bool GameIsOver = false;
    /// <summary>
    /// SetActive if player loses
    /// </summary>
    public GameObject GameOverPanel;
    public void GameOver()
    {
        if (GameIsOver)
            return;
        Debug.Log("Game over!");
        GameIsOver = true;
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
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
            , Random.Range(SpawnPosTopLeft.position.y, SpawnPosBottomRight.position.y) + 15f    // Drop in from above!
            , Random.Range(SpawnPosTopLeft.position.z, SpawnPosBottomRight.position.z));
    }


    public void RegisterTarget(GameObject d)
    {
        if (!TargetsPerTeam.ContainsKey(d.tag))
            TargetsPerTeam[d.tag] = new HashSet<GameObject>();
        TargetsPerTeam[d.tag].Add(d);
    }
    public void RemoveTarget(GameObject d)
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
        else if (Input.GetKeyDown(KeyCode.Equals))
        {
            SpawnPrefab(PlayerPrefab, true);
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            SpawnPrefab(EnemyPrefab, true);
        }
    }

    private int someNumber = 69;
    public static FightManager Instance;
    void Awake()
    {
        Instance = this;
        StartCoroutine(ProgressCoroutine());
        StartCoroutine(TutorialCo());
        /*
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(EnemyAnimator, "PunchLeft"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(PlayerAnimator, "PunchRight"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(EnemyAnimator, "PunchLeft"));
        AnimationManager.AddAnim(AnimationManager.Instance.PlayAndFinishAnimation(PlayerAnimator, "PunchRight"));*/
    }
}
