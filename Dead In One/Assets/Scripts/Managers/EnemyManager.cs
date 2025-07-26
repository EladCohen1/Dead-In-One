using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] MainBoardGrid mainBoardGrid;
    [SerializeField] TurnManager turnManager;

    [Header("Enemy Data")]
    [SerializeField] EnemyView enemyPawnPrefab;
    [SerializeField] EnemyView enemyKnightPrefab;

    [Header("Wave Data")]
    [SerializeField] int enemySpawnPerTurn = 2;
    [SerializeField] int enemySpawnTurnCD = 5;

    [Header("Spawn Weights")]
    [SerializeField] float pawnWeight;
    [SerializeField] float knightWeight;

    [Header("Active Enemies")]
    List<EnemyView> enemies = new();

    // Runtime Data
    private int enemySpawnTurnCDTimer;

    void Awake()
    {
        ServiceLocator.Register(this);
    }

    void Start()
    {
        enemySpawnTurnCDTimer = enemySpawnTurnCD;
    }

    void OnEnable()
    {
        turnManager.EnemyTurnStart += EnemyTurnHandler;
    }

    void OnDisable()
    {
        turnManager.EnemyTurnStart -= EnemyTurnHandler;
    }

    void EnemyTurnHandler()
    {
        SpawnEnemies();
        MoveEnemies();

        turnManager.EndEnemyTurn();
    }


    EnemyView SpawnEnemy(EnemyTypeEnum enemyType)
    {
        if (mainBoardGrid.valdEnemySpawnLocations.Count == 0)
            return null;

        int randomIndex = Random.Range(0, mainBoardGrid.valdEnemySpawnLocations.Count);
        Vector2Int spawnPos = mainBoardGrid.valdEnemySpawnLocations[randomIndex];

        EnemyView newEnemy;
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
        newEnemy.UpdatePos(spawnPos);
        return newEnemy;
    }
    void SpawnEnemies()
    {
        if (enemySpawnTurnCDTimer <= 0)
        {
            for (int i = 0; i < enemySpawnPerTurn; i++)
            {
                EnemyView newEnemy = SpawnEnemy(GetRandomWeightedEnemyType());
                if (newEnemy != null)
                    enemies.Add(newEnemy);
            }
            enemySpawnTurnCDTimer = enemySpawnTurnCD;
        }
        enemySpawnTurnCDTimer--;
    }

    void MoveEnemies()
    {
        foreach (EnemyView enemy in enemies)
        {
            enemy.MoveTowardsPlayer();
        }
    }
    public EnemyTypeEnum GetRandomWeightedEnemyType()
    {
        // Define weights
        Dictionary<EnemyTypeEnum, float> weights = new Dictionary<EnemyTypeEnum, float>
    {
        { EnemyTypeEnum.Pawn, pawnWeight },
        { EnemyTypeEnum.Knight, knightWeight }
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
