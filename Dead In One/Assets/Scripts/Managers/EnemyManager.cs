using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] MainBoardGrid mainBoardGrid;
    [SerializeField] TurnManager turnManager;

    [Header("Enemy Data")]
    [SerializeField] List<EnemyWeightPair> enemyWeightPairs;
    Dictionary<EnemyTypeEnum, EnemyWeightPair> enemyDic = new();

    [Header("Wave Data")]
    [SerializeField] int enemySpawnPerTurn = 2;
    [SerializeField] int enemySpawnTurnCD = 5;


    [Header("Active Enemies")]
    List<EnemyView> enemies = new();

    // Runtime Data
    private int enemySpawnTurnCDTimer;

    //bool didSpawn = false;

    void Awake()
    {
        ServiceLocator.Register(this);
        InitEnemyDic();
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
        ActEnemies();
        SpawnEnemies();
        // if (!didSpawn)
        // {
        //     EnemyView newEnemy = SpawnEnemy(enemyDic.GetValueOrDefault(EnemyTypeEnum.Pawn).enemyView);
        //     if (newEnemy != null)
        //     {
        //         enemies.Add(newEnemy);
        //         didSpawn = true;
        //     }
        // }

        turnManager.EndEnemyTurn();
    }


    public EnemyView SpawnEnemy(EnemyView enemyView)
    {
        if (mainBoardGrid.valdEnemySpawnLocations.Count == 0)
            return null;

        int randomIndex = Random.Range(0, mainBoardGrid.valdEnemySpawnLocations.Count);
        Vector2Int spawnPos = mainBoardGrid.valdEnemySpawnLocations[randomIndex];

        Vector3 worldPos = mainBoardGrid.GetWorldPosByGridPos(spawnPos);
        EnemyView newEnemy = Instantiate(enemyView, worldPos, Quaternion.identity);
        newEnemy.UpdatePos(spawnPos);

        return newEnemy;
    }

    void SpawnEnemies()
    {
        if (enemySpawnTurnCDTimer <= 0)
        {
            for (int i = 0; i < enemySpawnPerTurn; i++)
            {
                EnemyView newEnemy = SpawnEnemy(GetRandomWeightedEnemy());
                if (newEnemy != null)
                    enemies.Add(newEnemy);
            }
            enemySpawnTurnCDTimer = enemySpawnTurnCD;
        }
        else
            enemySpawnTurnCDTimer--;
    }

    void ActEnemies()
    {
        foreach (EnemyView enemy in enemies)
        {
            enemy.enemyController.Act();
        }
    }
    public EnemyView GetRandomWeightedEnemy()
    {
        float totalWeight = enemyWeightPairs.Sum(pair => pair.weight);
        float rand = Random.Range(0f, totalWeight);

        float cumulative = 0f;
        foreach (var pair in enemyWeightPairs)
        {
            cumulative += pair.weight;
            if (rand <= cumulative)
                return pair.enemyView;
        }

        // Fallback
        return enemyWeightPairs.Count > 0 ? enemyWeightPairs[0].enemyView : null;
    }

    // Utils
    void InitEnemyDic()
    {
        foreach (EnemyWeightPair enemyWeightPair in enemyWeightPairs)
        {
            enemyDic.Add(enemyWeightPair.enemyView.type, enemyWeightPair);
        }
    }


    // public Actions
    public void RemoveEnemy(EnemyView enemyView)
    {
        enemies.Remove(enemyView);
    }
}

[System.Serializable]
public struct EnemyWeightPair
{
    public EnemyView enemyView;
    public float weight;
}