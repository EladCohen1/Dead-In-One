using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Dependencies")]
    MainBoardGrid mainBoardGrid;
    TurnManager turnManager;

    [Header("Enemy Data")]
    [SerializeField] GameObject enemyPawnPrefab;
    [SerializeField] GameObject enemyKnightPrefab;

    [Header("Wave Data")]
    int enemySpawnPerTurn = 2;
    int enemySpawnTurnCD = 5;

    [Header("Active Enemies")]
    List<EnemyMoveController> enemies = new();

    // Runtime Data
    private int enemySpawnTurnCDTimer;

    void Awake()
    {
        mainBoardGrid = FindAnyObjectByType<MainBoardGrid>();
        turnManager = FindAnyObjectByType<TurnManager>();
    }

    void OnEnable()
    {
        turnManager.EnemyTurnStart += EnemyTurnHandler;
    }

    void OnDisable()
    {
        turnManager.EnemyTurnStart -= EnemyTurnHandler;
    }

    EnemyMoveController SpawnEnemy(EnemyTypeEnum enemyType)
    {
        int randomIndex = Random.Range(0, mainBoardGrid.valdEnemySpawnLocations.Count);
        if (mainBoardGrid.valdEnemySpawnLocations.Count == 0)
            return null;
        Vector2Int spawnPos = mainBoardGrid.valdEnemySpawnLocations[randomIndex];

        GameObject newEnemy;

        switch (enemyType)
        {
            case EnemyTypeEnum.Pawn:
                newEnemy = Instantiate(enemyPawnPrefab, mainBoardGrid.GetWorldPosByGridPos(spawnPos), Quaternion.identity);
                break;
            case EnemyTypeEnum.Knight:
                newEnemy = Instantiate(enemyKnightPrefab, mainBoardGrid.GetWorldPosByGridPos(spawnPos), Quaternion.identity);
                break;
            default:
                newEnemy = Instantiate(enemyPawnPrefab, mainBoardGrid.GetWorldPosByGridPos(spawnPos), Quaternion.identity);
                break;
        }

        mainBoardGrid.OccupyTile(spawnPos, newEnemy);

        // Update EnemyBase Data
        EnemyMoveController enemyBase = newEnemy.GetComponent<EnemyMoveController>();
        if (enemyBase == null)
            return null;

        enemyBase.currentPos = spawnPos;
        return enemyBase;
    }

    void EnemyTurnHandler()
    {
        // Spawning
        if (enemySpawnTurnCDTimer <= 0)
        {
            for (int i = 0; i < enemySpawnPerTurn; i++)
            {
                EnemyMoveController newEnemy = SpawnEnemy(GetRandomWeightedEnemyType());
                if (newEnemy != null)
                    enemies.Add(newEnemy);
            }
            enemySpawnTurnCDTimer = enemySpawnTurnCD;
        }
        enemySpawnTurnCDTimer--;

        // Moving
        foreach (EnemyMoveController enemy in enemies)
        {
            enemy.MoveTowardsPlayer();
        }

        turnManager.EndEnemyTurn();
    }

    public EnemyTypeEnum GetRandomWeightedEnemyType()
    {
        // Define weights
        Dictionary<EnemyTypeEnum, float> weights = new Dictionary<EnemyTypeEnum, float>
    {
        { EnemyTypeEnum.Pawn, 0.99f },
        { EnemyTypeEnum.Knight, 0.01f }
    };

        // Get total weight
        float total = weights.Values.Sum();
        float rand = Random.Range(0f, total);

        float cumulative = 0f;
        foreach (var kvp in weights)
        {
            cumulative += kvp.Value;
            if (rand <= cumulative)
                return kvp.Key;
        }

        // Fallback (should never reach here)
        return EnemyTypeEnum.Pawn;
    }
}
