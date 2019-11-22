#pragma warning disable 0649
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private float boardWidth = 6f;
    [SerializeField] private float boardHeight = 4f;
    [SerializeField] private float boardDepth = 1f;

    [SerializeField] private int horizontalCutAmount = 0;
    [SerializeField] private int verticalCutAmount = 0;

    [SerializeField] private GridSettings gridSettings;
    
    private readonly MeshData boardMesh = new MeshData();

    private MeshFilter _meshFilter;
    private MeshFilter meshFilter
    {
        get
        {
            if (!_meshFilter)
                _meshFilter = GetComponent<MeshFilter>();
            return _meshFilter;
        }
    }

    private void Awake() => CreatePlane();

    public void Generate() => CreatePlane();

    private void CreatePlane()
    {
        // Create arrays with cut positions.
        float[] horizontalCuts = new float[horizontalCutAmount];
        float[] verticalCuts = new float[verticalCutAmount];
        CutGenerator.Cut(ref horizontalCuts, ref verticalCuts, boardWidth, boardHeight);

        // Create array with components to cut.
        gridSettings.width = horizontalCutAmount + 1;
        gridSettings.height = verticalCutAmount + 1;
        int[,] grid = GridGenerator.PopulateGrid(gridSettings);
        
        // Create board mesh.
        boardMesh.Clear();
        
        for (int x = 0; x < horizontalCutAmount + 1; x++)
        for (int y = 0; y < verticalCutAmount + 1; y++)
        {
            if (grid[x, y] != 0)
                continue;

            boardMesh.vertices.Add(GetVertexPosition(x, y, horizontalCuts, verticalCuts));
            boardMesh.vertices.Add(GetVertexPosition(x, y + 1, horizontalCuts, verticalCuts));
            boardMesh.vertices.Add(GetVertexPosition(x + 1, y, horizontalCuts, verticalCuts));
            boardMesh.vertices.Add(GetVertexPosition(x + 1, y + 1, horizontalCuts, verticalCuts));
            
            int vertexCount = boardMesh.vertices.Count;
            boardMesh.AddQuad(vertexCount - 4, vertexCount - 3, vertexCount - 2, vertexCount - 1);
        }
        
        meshFilter.sharedMesh = boardMesh.CreateMesh();
    }

    private Vector3 GetVertexPosition(int x, int y, float[] horizontalCuts, float[] verticalCuts)
    {
        return new Vector3(
            x == 0 ? 0f : x <= horizontalCutAmount ? horizontalCuts[x - 1] : boardWidth,
            y == 0 ? 0f : y <= verticalCutAmount ? verticalCuts[y - 1] : boardHeight
        );
    }
    
    private void OnValidate()
    {
        boardWidth = Mathf.Max(0.01f, boardWidth);
        boardHeight = Mathf.Max(0.01f, boardHeight);
        boardDepth = Mathf.Max(0.01f, boardDepth);
        
        horizontalCutAmount = Mathf.Max(0, horizontalCutAmount);
        verticalCutAmount = Mathf.Max(0, verticalCutAmount);
    }
}
