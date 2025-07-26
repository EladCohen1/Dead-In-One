using System;
using UnityEngine;

public class MainBoardGenerator : MonoBehaviour
{
    [Header("Board Data")]
    [SerializeField] float tileSize;
    [SerializeField] int tileCountX;
    [SerializeField] int tileCountY;

    [Header("Art")]
    [SerializeField] Material blackTileMat;
    [SerializeField] Material whiteTileMat;
    [Space(40)]
    [SerializeField] Material tileAttackedMat;

    // Logic
    private TileManager[,] tiles;

    public TileManager[,] GenerateGrid()
    {
        tiles = new TileManager[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                tiles[x, y] = GenerateTile(tileSize, x, y);
        return tiles;
    }

    private TileManager GenerateTile(float tileSize, int xPos, int yPos)
    {
        GameObject tileObject = new GameObject($"X: {xPos}, Y: {yPos}");
        tileObject.transform.parent = transform;

        tileObject.transform.position = new Vector3(xPos * tileSize, 0, yPos * tileSize);

        Mesh mesh = new();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;

        MeshRenderer renderer = tileObject.AddComponent<MeshRenderer>();
        renderer.material = ((xPos + yPos) % 2 == 1) ? blackTileMat : whiteTileMat;

        float half = tileSize / 2f;
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-half, 0, -half);
        vertices[1] = new Vector3(half, 0, -half);
        vertices[2] = new Vector3(-half, 0, half);
        vertices[3] = new Vector3(half, 0, half);

        int[] tris = new int[] { 0, 2, 1, 2, 3, 1 };

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.AddComponent<BoxCollider>();
        TileManager tileManager = tileObject.AddComponent<TileManager>();
        tileManager.Init(renderer, renderer.material, tileAttackedMat);

        return tileManager;
    }
}
