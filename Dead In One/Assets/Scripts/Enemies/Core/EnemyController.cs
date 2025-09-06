using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [Header("Line Attack")]
    private List<Vector2Int> lineAttackTargets = new();

    [Header("Drop Prefabs")]
    [SerializeField] ExpDropController expDropPrefab;
    [SerializeField] HealDropController hpDropPrefab;

    void Awake()
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
        if (TryPrepareAttack())
            return;

        enemyView.MoveTowardsPlayer();

        if (TryPrepareAttack())
            return;
    }

    void PrepareToAttack()
    {
        // Attack Line Setup
        lineAttackTargets.Clear();

        // Attacked Tiles Clear
        CleanUpAttackTiles();

        // Get Best Matching Attackable Positions
        attackPositions = GetClosestAttackablePositions(enemyModel.GetNumberOfAttacks());

        foreach (Vector2Int attackPos in attackPositions)
        {
            Vector2Int delta = attackPos - enemyView.currentPos;
            if (GridCalculator.IsLineMovement(delta))
                HandlePrepareStraightAttack(attackPos);
            else
                HandlePrepareNonStraightAttack(attackPos);
        }
        if (attackPositions.Count > 0)
            isAttackPrepared = true;
    }
    void HandlePrepareNonStraightAttack(Vector2Int attackPos)
    {
        if (!mainBoardGrid.IsInRange(attackPos))
            return;

        mainBoardGrid.GetTile(attackPos).AttackTile(this);
        preparedAttacks.Add(attackPos);
    }
    void HandlePrepareStraightAttack(Vector2Int attackPos)
    {
        lineAttackTargets.Add(attackPos);

        int safety = 0;
        Vector2Int delta = attackPos - enemyView.currentPos;
        Vector2Int step = new Vector2Int(Math.Sign(delta.x), Math.Sign(delta.y));

        Vector2Int pos = enemyView.currentPos + step;
        while (pos != attackPos + step && safety < 10)
        {
            if (mainBoardGrid.IsInRange(pos))
            {
                mainBoardGrid.GetTile(pos).AttackTile(this);
                preparedAttacks.Add(pos);
            }
            pos += step;
            safety++;
        }
        if (safety >= 10)
            Debug.LogWarning($"Safety Triggered, Tried to create a straight line from: {delta}");
    }

    void AttackAction()
    {
        bool isHit = false;
        Vector2Int playerPos = playerController.GetCurrentPosition();

        foreach (Vector2Int pos in preparedAttacks)
            if (pos == playerPos) isHit = true;

        if (isHit)
        {
            playerController.TakeDamage(enemyModel.GetDamage());

            foreach (Vector2Int pos in preparedAttacks)
                mainBoardGrid.GetTile(pos).FlashAttackedMaterial(enemyView.attackingMat);
        }

        isAttackPrepared = false;
        CleanUpAttackTiles();

        MoveAfterAttack();
    }

    // Public Actions
    public void TakeDamage(int damage, Material attackedMat = null)
    {
        enemyModel.TakeDamage(damage);
        enemyView.FlashAttacked(attackedMat);
        if (enemyModel.GetCurrentHP() <= 0)
            Die();
    }

    // Utils
    bool IsPlayerCloseToAttackRange()
    {
        foreach (Vector2Int validAttack in enemyView.validAttackDirections)
        {
            if (GridCalculator.IsLineMovement(validAttack))
            {
                if (HandleIsPlayerCloseToAttackRangeLine(validAttack))
                    return true;
            }
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
        if (IsPlayerCloseToAttackRange())
        {
            PrepareToAttack();
            return true;
        }
        return false;
    }

    void Die()
    {
        DeathCleanUp();
        HandleDrops();
        Destroy(gameObject);
    }
    void DeathCleanUp()
    {
        mainBoardGrid.RemoveEntity(enemyView);
        EnemyManager enemyManager = ServiceLocator.Get<EnemyManager>();
        enemyManager.RemoveEnemy(enemyView);
        CleanUpAttackTiles();
    }
    void CleanUpAttackTiles()
    {
        foreach (Vector2Int pos in preparedAttacks)
        {
            if (mainBoardGrid.IsInRange(pos))
                mainBoardGrid.GetTile(pos).RemoveAttacker(this);
        }
        preparedAttacks.Clear();
    }
    void MoveAfterAttack()
    {
        if (lineAttackTargets.Count == 0)
            return;

        Vector2Int closestTargetToPlayer = lineAttackTargets[0];

        foreach (Vector2Int target in lineAttackTargets)
        {
            if ((closestTargetToPlayer - playerController.GetCurrentPosition()).sqrMagnitude
            > (target - playerController.GetCurrentPosition()).sqrMagnitude)
            {
                closestTargetToPlayer = target;
            }
        }

        if (mainBoardGrid.IsInRange(closestTargetToPlayer) && mainBoardGrid.entitiesOnTiles[closestTargetToPlayer.x, closestTargetToPlayer.y] == null)
            enemyView.UpdatePos(closestTargetToPlayer);
    }
    void HandleDrops()
    {
        if (RollChance(enemyModel.GetChanceToDropHp()))
        {
            DropHp();
            return;
        }
        if (RollChance(enemyModel.GetChanceToDropExp()))
        {
            DropExp();
            return;
        }
    }
    void DropHp()
    {
        Vector3 pos = mainBoardGrid.GetWorldPosByGridPos(enemyView.currentPos);
        pos.y = 0.5f;
        HealDropController healDropController = Instantiate(hpDropPrefab);
        healDropController.transform.position = pos;
        healDropController.HpHeld = enemyModel.GetHpDropped();
        mainBoardGrid.GetTile(enemyView.currentPos).AddHpDropped(healDropController);
    }
    void DropExp()
    {
        Vector3 pos = mainBoardGrid.GetWorldPosByGridPos(enemyView.currentPos);
        pos.y = 0.5f;
        ExpDropController ExpDropController = Instantiate(expDropPrefab);
        ExpDropController.transform.position = pos;
        ExpDropController.ExpHeld = enemyModel.GetExpDropped();
        mainBoardGrid.GetTile(enemyView.currentPos).AddExpDropped(ExpDropController);
    }

    public bool RollChance(float percent)
    {
        return UnityEngine.Random.Range(0f, 100f) < percent;
    }

    List<Vector2Int> GetClosestAttackablePositions(int count)
    {
        HashSet<Vector2Int> attackablePositions = new();

        // Add positions from valid attack directions
        foreach (Vector2Int dir in enemyView.validAttackDirections)
        {
            if (GridCalculator.IsLineMovement(dir))
            {
                if (HandleIsPlayerCloseToAttackRangeLine(dir))
                    attackablePositions.Add(enemyView.currentPos + dir);
            }
            else if (HandleIsPlayerCloseToAttackRangeNonLine(dir))
                attackablePositions.Add(enemyView.currentPos + dir);
        }

        // Sort positions by distance to playerPos
        return attackablePositions
            .OrderBy(pos => (pos - playerController.GetCurrentPosition()).sqrMagnitude) // Use sqrMagnitude for performance
            .Take(count)
            .ToList();
    }
}
