using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Dependencies")]
    protected MainBoardGrid mainBoardGrid;

    // Data
    [NonSerialized] public Vector2Int currentPos;
    public Vector2Int[] validMoveDirections;

    void Awake()
    {
        mainBoardGrid = FindAnyObjectByType<MainBoardGrid>();
    }

    public void MoveTowardsPlayer()
    {
        Vector2Int targetPos = FindMovementPosition();

        // Logic Grid
        mainBoardGrid.MoveToTile(currentPos, targetPos, gameObject);

        // Visual Move
        Vector3 targetWorldPos = mainBoardGrid.GetWorldPosByGridPos(targetPos);
        transform.DOMove(targetWorldPos, 0.3f).SetEase(Ease.OutQuad);
        currentPos = targetPos;
    }

    private Vector2Int FindMovementPosition()
    {
        float minDistance = Mathf.Infinity;

        foreach (var dir in validMoveDirections)
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
