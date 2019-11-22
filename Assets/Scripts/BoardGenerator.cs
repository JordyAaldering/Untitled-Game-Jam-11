using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private float boardWidth = 6f;
    [SerializeField] private float boardHeight = 4f;
    [SerializeField] private float boardDepth = 1f;

    [SerializeField] private int horizontalCuts = 0;
    [SerializeField] private int verticalCuts = 0;

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

    private void Awake()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        Vector3 v0 = new Vector3(-boardWidth * 0.5f, -boardHeight * 0.5f, boardDepth * 0.5f);
        Vector3 v1 = new Vector3(boardWidth * 0.5f, -boardHeight * 0.5f, boardDepth * 0.5f);
        Vector3 v2 = new Vector3(boardWidth * 0.5f, -boardHeight * 0.5f, -boardDepth * 0.5f);
        Vector3 v3 = new Vector3(-boardWidth * 0.5f, -boardHeight * 0.5f, -boardDepth * 0.5f);
        Vector3 v4 = new Vector3(-boardWidth * 0.5f, boardHeight * 0.5f, boardDepth * 0.5f);
        Vector3 v5 = new Vector3(boardWidth * 0.5f, boardHeight * 0.5f, boardDepth * 0.5f);
        Vector3 v6 = new Vector3(boardWidth * 0.5f, boardHeight * 0.5f, -boardDepth * 0.5f);
        Vector3 v7 = new Vector3(-boardWidth * 0.5f, boardHeight * 0.5f, -boardDepth * 0.5f);

        Vector3[] vertices =
        {
            v0, v1, v2, v3, // Bottom
            v7, v4, v0, v3, // Left
            v4, v5, v1, v0, // Front
            v6, v7, v3, v2, // Back
            v5, v6, v2, v1, // Right
            v7, v6, v5, v4  // Top
        };

        Vector3 u = Vector3.up;
        Vector3 d = Vector3.down;
        Vector3 f = Vector3.forward;
        Vector3 b = Vector3.back;
        Vector3 l = Vector3.left;
        Vector3 r = Vector3.right;

        Vector3[] normals =
        {
            d, d, d, d, // Bottom
            l, l, l, l, // Left
            f, f, f, f, // Front
            b, b, b, b, // Back
            r, r, r, r, // Right
            u, u, u, u  // Top
        };

        Vector2 u00 = new Vector2(0f, 0f);
        Vector2 u10 = new Vector2(1f, 0f);
        Vector2 u01 = new Vector2(0f, 1f);
        Vector2 u11 = new Vector2(1f, 1f);

        Vector2[] uvs =
        {
            u11, u01, u00, u10, // Bottom
            u11, u01, u00, u10, // Left
            u11, u01, u00, u10, // Front
            u11, u01, u00, u10, // Back
            u11, u01, u00, u10, // Right
            u11, u01, u00, u10  // Top
        };

        int[] triangles =
        {
            3,  1,  0,  3,  2,  1,  // Bottom
            7,  5,  4,  7,  6,  5,  // Left
            11, 9,  8,  11, 10, 9,  // Front
            15, 13, 12, 15, 14, 13, // Back
            19, 17, 16, 19, 18, 17, // Right
            23, 21, 20, 23, 22, 21  // Top
        };

        boardMesh.Clear();
        
        boardMesh.vertices = new List<Vector3>(vertices);
        boardMesh.triangles = new List<int>(triangles);
        boardMesh.normals = new List<Vector3>(normals);
        boardMesh.uvs = new List<Vector2>(uvs);
        
        meshFilter.sharedMesh = boardMesh.CreateMesh();
    }

    private void CutBoard()
    {
        
    }
    
    private void OnValidate()
    {
        boardWidth = Mathf.Max(0.01f, boardWidth);
        boardHeight = Mathf.Max(0.01f, boardHeight);
        boardDepth = Mathf.Max(0.01f, boardDepth);
        
        horizontalCuts = Mathf.Max(0, horizontalCuts);
        verticalCuts = Mathf.Max(0, verticalCuts);
    }
}
