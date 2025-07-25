using System;
using System.Collections.Generic;
using UnityEngine;

public class MainBoardGrid : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] MainBoardGenerator mainBoardGenerator;

    // Data
    private TileManager[,] tiles;
    public EntityView[,] entitiesOnTiles;
    public int[,] playerDistanceField;
    [NonSerialized] public List<Vector2Int> valdEnemySpawnLocations = new();

    // Events
    public event Action OnGridGeneratedEvent;

    void Awake()
    {
        ServiceLocator.Register(this);
    }

    void Start()
    {
        tiles = mainBoardGenerator.GenerateGrid();
        entitiesOnTiles = new EntityView[tiles.GetLength(0), tiles.GetLength(1)];
        playerDistanceField = new int[tiles.GetLength(0), tiles.GetLength(1)];
        OnGridGeneratedEvent?.Invoke();
    }

    // Utils
    public Vector3 GetWorldPosByGridPos(Vector2Int gridPos)
    {
        return tiles[gridPos.x, gridPos.y].transform.position;
    }
    public bool IsInRange(Vector2Int gridPos)
    {
        int width = tiles.GetLength(0);  // X dimension
        int height = tiles.GetLength(1); // Y dimension

        return gridPos.x >= 0 && gridPos.x < width &&
               gridPos.y >= 0 && gridPos.y < height;
    }

    public void OccupyTile(Vector2Int location, EntityView entity)
    {
        entitiesOnTiles[location.x, location.y] = entity;
    }
    public void ClearTile(Vector2Int location)
    {
        entitiesOnTiles[location.x, location.y] = null;
    }
    public bool MoveToTile(EntityView entity, Vector2Int destination)
    {
        if (!IsInRange(destination))
            return false;

        if (entitiesOnTiles[destination.x, destination.y] != null)
            return false;

        if (entity.currentPos.x != -1)
            ClearTile(entity.currentPos);
        OccupyTile(destination, entity);

        return true;
    }

    public TileManager GetTile(Vector2Int pos)
    {
        return tiles[pos.x, pos.y];
    }

    // Player Distance Field
    public void GenerateDistanceField(Vector2Int origin)
    {
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        int[,] distanceField = new int[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                distanceField[x, y] = int.MaxValue;

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(origin);
        distanceField[origin.x, origin.y] = 0;

        Vector2Int[] directions = new Vector2Int[]
        {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();
            int currentDist = distanceField[current.x, current.y];

            // Populating validEnemySpawnLocations at distance 30 of the player
            if (currentDist > 30 && currentDist < 60)
                valdEnemySpawnLocations.Add(current);

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;
                if (neighbor.x < 0 || neighbor.x >= width || neighbor.y < 0 || neighbor.y >= height)
                    continue;

                if (distanceField[neighbor.x, neighbor.y] > currentDist + 1)
                {
                    distanceField[neighbor.x, neighbor.y] = currentDist + 1;
                    frontier.Enqueue(neighbor);
                }
            }
        }

        playerDistanceField = distanceField;
    }
}
