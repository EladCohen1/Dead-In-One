using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Currently Using move positions to attack
// TODO: Move to attack positions
public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] EnemyModel enemyModel;
    [SerializeField] EnemyView enemyView;

    [Header("Dependencies")]
    PlayerController playerController;
    MainBoardGrid mainBoardGrid;

    [Header("Runtime Data")]
    public bool isAttackPrepared { get; private set; }
    [NonSerialized] public List<Vector2Int> attackPositions;
    Vector2Int[] allAdjacentDirs = new Vector2Int[]
    {
        new Vector2Int(-1, -1), new Vector2Int( 0, -1), new Vector2Int( 1, -1),
        new Vector2Int(-1,  0),                         new Vector2Int( 1,  0),
        new Vector2Int(-1,  1), new Vector2Int( 0,  1), new Vector2Int( 1,  1),
    };

    void Start()
    {
        mainBoardGrid = ServiceLocator.Get<MainBoardGrid>();
        playerController = ServiceLocator.Get<PlayerController>();
    }

    public void Act()
    {
        if (isAttackPrepared)
            AttackAction();
        else
            MoveAction();
    }

    void MoveAction()
    {
        if (IsPlayerInAttackRange() || IsPlayerAdjacent())
        {
            PrepareToAttack();
            return;
        }

        enemyView.MoveTowardsPlayer();

        if (IsPlayerInAttackRange() || IsPlayerAdjacent())
        {
            PrepareToAttack();
            return;
        }
    }

    void PrepareToAttack()
    {
        attackPositions = GetClosestAttackablePositions(enemyModel.GetNumberOfAttacks());

        foreach (Vector2Int attackPos in attackPositions)
        {
            mainBoardGrid.GetTile(attackPos).SetAttackedTile();
        }
        isAttackPrepared = true;
    }

    void AttackAction()
    {
        bool isHit = false;
        Vector2Int playerPos = playerController.GetCurrentPosition();
        foreach (Vector2Int pos in attackPositions)
        {
            mainBoardGrid.GetTile(pos).RevertMatToBase();
            if (pos == playerPos)
            {
                isHit = true;
                break;
            }
        }

        if (isHit)
        {
            playerController.TakeDamage(enemyModel.GetDamage());
        }
        isAttackPrepared = false;
    }

    // Public Actions
    public void TakeDamage(int damage)
    {
        enemyModel.TakeDamage(damage);
        if (enemyModel.GetCurrentHP() <= 0)
            Destroy(gameObject);
    }

    // Utils
    bool IsPlayerInAttackRange()
    {
        if (playerController == null)
            playerController = ServiceLocator.Get<PlayerController>();

        Vector2Int delta = playerController.GetCurrentPosition() - enemyView.currentPos;
        return enemyView.validMoveDirections.Contains(delta);
    }
    bool IsPlayerAdjacent()
    {
        Vector2Int delta = playerController.GetCurrentPosition() - enemyView.currentPos;
        return Mathf.Abs(delta.x) <= 1 && Mathf.Abs(delta.y) <= 1 && delta != Vector2Int.zero;
    }
    void OnDestroy()
    {
        mainBoardGrid.RemoveEntity(enemyView);
        EnemyManager enemyManager = ServiceLocator.Get<EnemyManager>();
        enemyManager.RemoveEnemy(enemyView);

        if (isAttackPrepared)
            CleanUpAttackTiles();
    }
    void CleanUpAttackTiles()
    {
        foreach (Vector2Int pos in attackPositions)
        {
            mainBoardGrid.GetTile(pos).RevertMatToBase();
        }
    }

    List<Vector2Int> GetClosestAttackablePositions(int count)
    {
        HashSet<Vector2Int> attackablePositions = new();

        // Add positions from valid attack directions
        foreach (Vector2Int dir in enemyView.validAttackDirections)
        {
            attackablePositions.Add(enemyView.currentPos + dir);
        }

        foreach (Vector2Int dir in allAdjacentDirs)
        {
            attackablePositions.Add(enemyView.currentPos + dir);
        }

        // Sort positions by distance to playerPos
        return attackablePositions
            .OrderBy(pos => (pos - playerController.GetCurrentPosition()).sqrMagnitude) // Use sqrMagnitude for performance
            .Take(count)
            .ToList();
    }
}
