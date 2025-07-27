using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyView : EntityView
{
    [Header("Components")]
    public EnemyController enemyController;

    [Header("Materials")]
    [SerializeField] Material attackMaterial;
    [SerializeField] Material attackTileMaterial;


    [Header("Directions")]
    public Vector2Int[] validMoveDirections;
    public Vector2Int[] validAttackDirections;

    // Actions
    public void MoveTowardsPlayer()
    {
        Vector2Int targetPos = FindMovementPosition();

        UpdatePos(targetPos);
    }
    // public void PrepareToAttack()
    // {
    //     foreach (Vector2Int dir in validAttackDirections)
    //     {
    //         Vector2Int neighbor = currentPos + dir;

    //         // Bounds check
    //         if (neighbor.x < 0 || neighbor.x >= mainBoardGrid.playerDistanceField.GetLength(0) ||
    //             neighbor.y < 0 || neighbor.y >= mainBoardGrid.playerDistanceField.GetLength(1))
    //             continue;
    //     }
    //     isAttackPrepared = true;
    // }

    // Utils
    private Vector2Int FindMovementPosition()
    {
        float minDistance = Mathf.Infinity;
        foreach (Vector2Int dir in validMoveDirections)
        {
            Vector2Int neighbor = currentPos + dir;

            // Bounds check
            if (neighbor.x < 0 || neighbor.x >= mainBoardGrid.playerDistanceField.GetLength(0) ||
                neighbor.y < 0 || neighbor.y >= mainBoardGrid.playerDistanceField.GetLength(1))
                continue;

            int dist = mainBoardGrid.playerDistanceField[neighbor.x, neighbor.y];

            if (dist < minDistance && mainBoardGrid.entitiesOnTiles[neighbor.x, neighbor.y] == null)
            {
                minDistance = dist;
            }

        }
        if (minDistance == Mathf.Infinity)
            return currentPos;

        List<Vector2Int> possibleMovePos = new();
        foreach (Vector2Int dir in validMoveDirections)
        {
            Vector2Int neighbor = currentPos + dir;

            // Bounds check
            if (neighbor.x < 0 || neighbor.x >= mainBoardGrid.playerDistanceField.GetLength(0) ||
                neighbor.y < 0 || neighbor.y >= mainBoardGrid.playerDistanceField.GetLength(1))
                continue;

            int dist = mainBoardGrid.playerDistanceField[neighbor.x, neighbor.y];
            if (dist <= minDistance + 1 && mainBoardGrid.entitiesOnTiles[neighbor.x, neighbor.y] == null)
                possibleMovePos.Add(neighbor);
        }

        int randomIndex = UnityEngine.Random.Range(0, possibleMovePos.Count);

        return possibleMovePos[randomIndex];
    }
}
