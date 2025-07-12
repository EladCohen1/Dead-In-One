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

    // Logic
    private GameObject[,] tiles;

    public GameObject[,] GenerateGrid()
    {
        tiles = new GameObject[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                tiles[x, y] = GenerateTile(tileSize, x, y);
        return tiles;
    }

    private GameObject GenerateTile(float tileSize, int xPos, int yPos)
    {
        GameObject tileObject = new GameObject($"X: {xPos}, Y: {yPos}");
        tileObject.transform.parent = transform;

        tileObject.transform.position = new Vector3(xPos * tileSize, 0, yPos * tileSize);

        Mesh mesh = new();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;

        tileObject.AddComponent<MeshRenderer>().material = ((xPos + yPos) % 2 == 1) ? blackTileMat : whiteTileMat;

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

        return tileObject;
    }
}
