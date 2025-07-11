using System;
using UnityEngine;

public class MainBoardGrid : MonoBehaviour
{
    // Dependeincies
    private MainBoardGenerator mainBoardGenerator;

    // Data
    private GameObject[,] tiles;

    // Events
    public event Action OnGridGeneratedEvent;

    void Awake()
    {
        mainBoardGenerator = GetComponent<MainBoardGenerator>();
    }

    void Start()
    {
        tiles = mainBoardGenerator.GenerateGrid();
        OnGridGeneratedEvent?.Invoke();
    }

    // Utils
    public Vector3 GetWorldPosByGridPos(Vector2Int gridPos)
    {
        return tiles[gridPos.x, gridPos.y].transform.position;
    }
}
