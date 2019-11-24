#pragma warning disable 0649
using UnityEngine;

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
    
    [SerializeField] private Transform boardObject;
    [SerializeField] private Transform componentsParent;
    [SerializeField] private Material[] componentMaterials;
    
    private readonly MeshData board = new MeshData("Board Mesh");
    private BoardComponent[] components = new BoardComponent[0];
    
    private MeshFilter _boardMeshFilter;
    private MeshFilter boardMeshFilter
    {
        get
        {
            if (!_boardMeshFilter)
                _boardMeshFilter = boardObject.GetComponent<MeshFilter>();
            return _boardMeshFilter;
        }
    }
    
    private int[,] grid = new int[0, 0];
    private float[] horizontalCuts = new float[0];
    private float[] verticalCuts = new float[0];
    
    public void Generate() => CreateBoard();
    
    private void CreateBoard()
    {
        CutGenerator.Cut(ref horizontalCuts, ref verticalCuts, cutSettings);
        grid = GridGenerator.PopulateGrid(gridSettings);

        Clear();
        CreateMeshes();
        BuildMeshes();
    }
    
    private void Clear()
    {
        board.Clear();
        components = new BoardComponent[gridSettings.MaxComponents];
        
        int childCount = componentsParent.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(componentsParent.GetChild(i).gameObject);
        }
    }
    
    private void CreateMeshes()
    {
        for (int x = 0; x < horizontalCutAmount + 1; x++)
        for (int y = 0; y < verticalCutAmount + 1; y++)
        {
            int i = grid[x, y];
            if (i == 0)
            {
                board.vertices.Add(GetVertexPosition(x, y, horizontalCuts, verticalCuts));
                board.vertices.Add(GetVertexPosition(x, y + 1, horizontalCuts, verticalCuts));
                board.vertices.Add(GetVertexPosition(x + 1, y, horizontalCuts, verticalCuts));
                board.vertices.Add(GetVertexPosition(x + 1, y + 1, horizontalCuts, verticalCuts));

                int vertexCount = board.vertices.Count;
                board.AddQuad(vertexCount - 4, vertexCount - 3, vertexCount - 2, vertexCount - 1);
            }
            else
            {
                // Create a new mesh if there is none.
                if (components[i - 1] == null)
                    components[i - 1] = new BoardComponent("Component " + i);

                MeshData mesh = components[i - 1].meshData;
                
                // Add vertices.
                mesh.vertices.Add(GetVertexPosition(x, y, horizontalCuts, verticalCuts));
                mesh.vertices.Add(GetVertexPosition(x, y + 1, horizontalCuts, verticalCuts));
                mesh.vertices.Add(GetVertexPosition(x + 1, y, horizontalCuts, verticalCuts));
                mesh.vertices.Add(GetVertexPosition(x + 1, y + 1, horizontalCuts, verticalCuts));

                // Add triangles.
                int vertexCount = mesh.vertices.Count;
                components[i - 1].meshData.AddQuad(vertexCount - 4, vertexCount - 3, vertexCount - 2, vertexCount - 1);
            }
        }
    }

    private void BuildMeshes()
    {
        boardMeshFilter.sharedMesh = board.CreateMesh();
        
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
                break;

            components[i].CreateObject(componentsParent);
            components[i].BuildMesh(componentMaterials[i % componentMaterials.Length]);
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
        
        _horizontalCutAmount = Mathf.Max(0, horizontalCutAmount);
        _verticalCutAmount = Mathf.Max(0, verticalCutAmount);
        
        boardWidth = _boardWidth;
        boardHeight = _boardHeight;
        
        horizontalCutAmount = _horizontalCutAmount;
        verticalCutAmount = _verticalCutAmount;
    }
}
