#pragma warning disable 0649
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
        // Create arrays with cut positions.
        float[] horizontalCuts = new float[horizontalCutAmount];
        float[] verticalCuts = new float[verticalCutAmount];
        CutGenerator.Cut(ref horizontalCuts, ref verticalCuts, boardWidth, boardHeight);

        // Create array with components to cut.
        gridSettings.width = horizontalCutAmount + 1;
        gridSettings.height = verticalCutAmount + 1;
        int[,] grid = GridGenerator.PopulateGrid(gridSettings);
        
        // Clear old components.
        for (int i = 0; i < componentObjects.Length; i++)
        {
            if (componentObjects[i] == null)
                break;

            DestroyImmediate(componentObjects[i]);
            componentObjects[i] = null;
        }

        // Create board mesh.
        boardMesh.Clear();
        componentMeshes = new MeshData[gridSettings.maxComponents];
        componentObjects = new GameObject[gridSettings.maxComponents];
        
        for (int x = 0; x < horizontalCutAmount + 1; x++)
        for (int y = 0; y < verticalCutAmount + 1; y++)
        {
            int i = grid[x, y];
            
            if (i == 0)
            {
                boardMesh.vertices.Add(GetVertexPosition(x, y, horizontalCuts, verticalCuts));
                boardMesh.vertices.Add(GetVertexPosition(x, y + 1, horizontalCuts, verticalCuts));
                boardMesh.vertices.Add(GetVertexPosition(x + 1, y, horizontalCuts, verticalCuts));
                boardMesh.vertices.Add(GetVertexPosition(x + 1, y + 1, horizontalCuts, verticalCuts));

                int vertexCount = boardMesh.vertices.Count;
                boardMesh.AddQuad(vertexCount - 4, vertexCount - 3, vertexCount - 2, vertexCount - 1);
            }
            else
            {
                if (componentMeshes[i - 1] == null)
                    componentMeshes[i - 1] = new MeshData("Component Mesh " + i);
                
                componentMeshes[i - 1].vertices.Add(GetVertexPosition(x, y, horizontalCuts, verticalCuts));
                componentMeshes[i - 1].vertices.Add(GetVertexPosition(x, y + 1, horizontalCuts, verticalCuts));
                componentMeshes[i - 1].vertices.Add(GetVertexPosition(x + 1, y, horizontalCuts, verticalCuts));
                componentMeshes[i - 1].vertices.Add(GetVertexPosition(x + 1, y + 1, horizontalCuts, verticalCuts));

                int vertexCount = componentMeshes[i - 1].vertices.Count;
                componentMeshes[i - 1].AddQuad(vertexCount - 4, vertexCount - 3, vertexCount - 2, vertexCount - 1);
            }
        }
        
        BoardBoardMeshFilter.sharedMesh = boardMesh.CreateMesh();
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
        boardWidth = Mathf.Max(0.01f, boardWidth);
        boardHeight = Mathf.Max(0.01f, boardHeight);
        boardDepth = Mathf.Max(0.01f, boardDepth);
        
        horizontalCutAmount = Mathf.Max(0, horizontalCutAmount);
        verticalCutAmount = Mathf.Max(0, verticalCutAmount);
    }
}
