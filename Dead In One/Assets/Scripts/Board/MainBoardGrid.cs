using System;
using UnityEngine;

public class MainBoardGrid : MonoBehaviour
{
    // Dependeincies
    private MainBoardGenerator mainBoardGenerator;

    // Data
    private GameObject[,] tiles;
    private GameObject[,] entitiesOnTiles;

    // Events
    public event Action OnGridGeneratedEvent;

    void Awake()
    {
        mainBoardGenerator = GetComponent<MainBoardGenerator>();
    }

    void Start()
    {
        tiles = mainBoardGenerator.GenerateGrid();
        entitiesOnTiles = new GameObject[tiles.GetLength(0), tiles.GetLength(1)];
        OnGridGeneratedEvent?.Invoke();
    }

    // Utils
    public Vector3 GetWorldPosByGridPos(Vector2Int gridPos)
    {
        return tiles[gridPos.x, gridPos.y].transform.position;
    }
    public void OccupyTile(Vector2Int location, GameObject entity)
    {
        entitiesOnTiles[location.x, location.y] = entity;
    }
    public void ClearTile(Vector2Int location)
    {
        entitiesOnTiles[location.x, location.y] = null;
    }
    public void MoveToTile(Vector2Int startLocation, Vector2Int destination, GameObject entity)
    {
        if (startLocation.x != -1)
            ClearTile(startLocation);
        OccupyTile(destination, entity);
    }
}
