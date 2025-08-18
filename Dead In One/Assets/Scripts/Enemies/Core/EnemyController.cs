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
    List<Vector2Int> preparedAttacks = new();
    Vector2Int[] allAdjacentDirs = new Vector2Int[]
    {
        new Vector2Int(-1, -1), new Vector2Int( 0, -1), new Vector2Int( 1, -1),
        new Vector2Int(-1,  0),                         new Vector2Int( 1,  0),
        new Vector2Int(-1,  1), new Vector2Int( 0,  1), new Vector2Int( 1,  1),
    };

    void Awake()
    {
        mainBoardGrid = ServiceLocator.TryGet<MainBoardGrid>();
        playerController = ServiceLocator.TryGet<PlayerController>();
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
        if (TryPrepareAttack())
            return;

        enemyView.MoveTowardsPlayer();

        if (TryPrepareAttack())
            return;
    }

    void PrepareToAttack()
    {
        preparedAttacks.Clear();
        attackPositions = GetClosestAttackablePositions(enemyModel.GetNumberOfAttacks());

        foreach (Vector2Int attackPos in attackPositions)
        {
            if (!IsLineMovement(attackPos))
                HandlePrepareNonStraightAttack(attackPos);
            HandlePrepareStraightAttack(attackPos);
        }
    }
    void HandlePrepareNonStraightAttack(Vector2Int attackPos)
    {
        if (!mainBoardGrid.IsInRange(attackPos))
            return;

        mainBoardGrid.GetTile(attackPos).SetAttackedTile();
        preparedAttacks.Add(attackPos);
    }
    void HandlePrepareStraightAttack(Vector2Int attackPos)
    {
        Vector2Int delta = attackPos - enemyView.currentPos;
        Vector2Int step = new Vector2Int(Math.Sign(delta.x), Math.Sign(delta.y));

        Vector2Int pos = enemyView.currentPos + step;
        while (pos != attackPos + step)
        {
            if (mainBoardGrid.IsInRange(pos))
            {
                mainBoardGrid.GetTile(pos).SetAttackedTile();
                preparedAttacks.Add(pos);
            }
            pos += step;
        }

        isAttackPrepared = true;
    }

    void AttackAction()
    {
        bool isHit = false;
        Vector2Int playerPos = playerController.GetCurrentPosition();

        foreach (Vector2Int pos in preparedAttacks)
        {
            if (mainBoardGrid.IsInRange(pos))
                HandleTileAttacked(pos);

            if (pos == playerPos) isHit = true;
        }

        if (isHit)
        {
            playerController.TakeDamage(enemyModel.GetDamage());
        }
        isAttackPrepared = false;
        preparedAttacks.Clear();
    }
    void HandleTileAttacked(Vector2Int pos)
    {
        mainBoardGrid.GetTile(pos).RevertMatToBase();
    }


    // Public Actions
    public void TakeDamage(int damage)
    {
        enemyModel.TakeDamage(damage);
        if (enemyModel.GetCurrentHP() <= 0)
            Destroy(gameObject);
    }

    // Utils
    bool IsPlayerCloseToAttackRange()
    {
        foreach (Vector2Int validAttack in enemyView.validAttackDirections)
        {
            if (IsLineMovement(validAttack) && HandleIsPlayerCloseToAttackRangeLine(validAttack))
                return true;
            else if (HandleIsPlayerCloseToAttackRangeNonLine(validAttack))
                return true;
        }
        return false;
    }
    bool HandleIsPlayerCloseToAttackRangeLine(Vector2Int validAttack)
    {
        Vector2Int step = new Vector2Int(Math.Sign(validAttack.x), Math.Sign(validAttack.y));
        if (step == Vector2Int.zero) return false;

        Vector2Int start = enemyView.currentPos + step;
        Vector2Int endExclusive = enemyView.currentPos + validAttack + step;

        for (Vector2Int pos = start; pos != endExclusive; pos += step)
        {
            Debug.Log(step);
            Debug.Log(pos);
            if (mainBoardGrid.IsInRange(pos)
            && mainBoardGrid.playerDistanceField[pos.x, pos.y] <= enemyModel.GetAttackRangeGrace())
                return true;

        }
        return false;
    }
    bool HandleIsPlayerCloseToAttackRangeNonLine(Vector2Int validAttack)
    {
        Vector2Int newPos = enemyView.currentPos + validAttack;
        if (!mainBoardGrid.IsInRange(newPos))
            return false;

        if (mainBoardGrid.playerDistanceField[newPos.x, newPos.y] <= enemyModel.GetAttackRangeGrace())
            return true;

        return false;
    }
    bool TryPrepareAttack()
    {
        if (IsPlayerAdjacent() || IsPlayerCloseToAttackRange())
        {
            PrepareToAttack();
            return true;
        }
        return false;
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
        foreach (Vector2Int pos in preparedAttacks)
        {
            if (mainBoardGrid.IsInRange(pos))
                mainBoardGrid.GetTile(pos).RevertMatToBase();
        }
        preparedAttacks.Clear();
    }

    bool IsLineMovement(Vector2Int dir)
    {
        if (dir == Vector2Int.zero) return false;

        // Horizontal, vertical, or perfect diagonal
        return dir.x == 0 || dir.y == 0 || Mathf.Abs(dir.x) == Mathf.Abs(dir.y);
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
