#pragma warning disable 0649
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private float _boardWidth = 6f;
    private float boardWidth
    {
        get => _boardWidth;
        set
        {
            cutSettings.width = value;
            _boardWidth = value;
        }
    }
    
    [SerializeField] private float _boardHeight = 4f;
    private float boardHeight
    {
        get => _boardHeight;
        set
        {
            cutSettings.height = value;
            _boardHeight = value;
        }
    }
    
    [SerializeField] private float boardDepth = 1f;

    [SerializeField] private int _horizontalCutAmount = 0;
    private int horizontalCutAmount
    {
        get => _horizontalCutAmount;
        set
        {
            gridSettings.width = value + 1;
            horizontalCuts = new float[value];
            _horizontalCutAmount = value;
        }
    }
    
    [SerializeField] private int _verticalCutAmount = 0;
    private int verticalCutAmount
    {
        get => _verticalCutAmount;
        set
        {
            gridSettings.height = value + 1;
            verticalCuts = new float[value];
            _verticalCutAmount = value;
        }
    }

    [SerializeField] private CutSettings cutSettings;
    [SerializeField] private GridSettings gridSettings;
    
    private int[,] grid = new int[0, 0];
    private float[] horizontalCuts = new float[0];
    private float[] verticalCuts = new float[0];
    
    private readonly MeshData boardMesh = new MeshData("Board Mesh");
    private MeshFilter _boardMeshFilter;
    private MeshFilter BoardBoardMeshFilter
    {
        get
        {
            if (!_boardMeshFilter)
                _boardMeshFilter = GetComponent<MeshFilter>();
            return _boardMeshFilter;
        }
    }

    [SerializeField] private GameObject componentPrefab;
    private MeshData[] componentMeshes = new MeshData[0];
    private GameObject[] componentObjects = new GameObject[0];

    private void Awake() => CreatePlane();

    public void Generate() => CreatePlane();

    private void CreatePlane()
    {
        Clear();
        
        CutGenerator.Cut(ref horizontalCuts, ref verticalCuts, cutSettings);
        grid = GridGenerator.PopulateGrid(gridSettings);

        CreateMeshes();
        BuildMeshes();
    }

    private void Clear()
    {
        // Clear old board mesh.
        boardMesh.Clear();
        
        // Remove old components.
        for (int i = 0; i < componentObjects.Length; i++)
        {
            if (componentObjects[i] == null)
                break;

            DestroyImmediate(componentObjects[i]);
            componentObjects[i] = null;
        }
        
        // Clear old components.
        componentMeshes = new MeshData[gridSettings.maxComponents];
        componentObjects = new GameObject[gridSettings.maxComponents];
    }

    private void CreateMeshes()
    {
        for (int x = 0; x < horizontalCutAmount + 1; x++)
        for (int y = 0; y < verticalCutAmount + 1; y++)
        {
            int i = grid[x, y];
            
            if (i == 0)
            {
                // Add vertices.
                boardMesh.vertices.Add(GetVertexPosition(x, y, horizontalCuts, verticalCuts));
                boardMesh.vertices.Add(GetVertexPosition(x, y + 1, horizontalCuts, verticalCuts));
                boardMesh.vertices.Add(GetVertexPosition(x + 1, y, horizontalCuts, verticalCuts));
                boardMesh.vertices.Add(GetVertexPosition(x + 1, y + 1, horizontalCuts, verticalCuts));

                // Add triangles.
                int vertexCount = boardMesh.vertices.Count;
                boardMesh.AddQuad(vertexCount - 4, vertexCount - 3, vertexCount - 2, vertexCount - 1);
            }
            else
            {
                // Subtract 1 to get component index.
                i--;
                
                // Create a new mesh if there is none.
                if (componentMeshes[i] == null)
                    componentMeshes[i] = new MeshData("Component Mesh " + i);
                
                // Add vertices.
                componentMeshes[i].vertices.Add(GetVertexPosition(x, y, horizontalCuts, verticalCuts));
                componentMeshes[i].vertices.Add(GetVertexPosition(x, y + 1, horizontalCuts, verticalCuts));
                componentMeshes[i].vertices.Add(GetVertexPosition(x + 1, y, horizontalCuts, verticalCuts));
                componentMeshes[i].vertices.Add(GetVertexPosition(x + 1, y + 1, horizontalCuts, verticalCuts));

                // Add triangles.
                int vertexCount = componentMeshes[i].vertices.Count;
                componentMeshes[i].AddQuad(vertexCount - 4, vertexCount - 3, vertexCount - 2, vertexCount - 1);
            }
        }
    }

    private void BuildMeshes()
    {
        // Build board mesh.
        BoardBoardMeshFilter.sharedMesh = boardMesh.CreateMesh();
        
        // Build component meshes.
        for (int i = 0; i < gridSettings.maxComponents; i++)
        {
            if (componentMeshes[i] == null)
                break;

            Transform t = transform;
            componentObjects[i] = Instantiate(componentPrefab, t.position, Quaternion.identity, t);

            Mesh mesh = componentMeshes[i].CreateMesh();
            componentObjects[i].GetComponent<MeshFilter>().sharedMesh = mesh;
            componentObjects[i].GetComponent<MeshCollider>().sharedMesh = mesh;
        }
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
        _boardWidth = Mathf.Max(0.01f, boardWidth);
        _boardHeight = Mathf.Max(0.01f, boardHeight);
        boardDepth = Mathf.Max(0.01f, boardDepth);

        boardWidth = _boardWidth;
        boardHeight = _boardHeight;
        
        _horizontalCutAmount = Mathf.Max(0, horizontalCutAmount);
        _verticalCutAmount = Mathf.Max(0, verticalCutAmount);

        horizontalCutAmount = _horizontalCutAmount;
        verticalCutAmount = _verticalCutAmount;
    }
}
