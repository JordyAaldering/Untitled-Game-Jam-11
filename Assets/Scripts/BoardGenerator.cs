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
    
    [SerializeField] private GameObject wallObject;
    [SerializeField] private Transform componentsParent;

    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material[] componentMaterials;

    private BoardWall _wall;
    private BoardWall wall => _wall ?? (_wall = new BoardWall(wallObject));

    private BoardComponent[] components = new BoardComponent[0];
    
    private MeshFilter _boardMeshFilter;
    private MeshFilter boardMeshFilter
    {
        get
        {
            if (!_boardMeshFilter)
                _boardMeshFilter = wallObject.GetComponent<MeshFilter>();
            return _boardMeshFilter;
        }
    }
    
    private int[,] grid = new int[0, 0];
    private float[] horizontalCuts = new float[0];
    private float[] verticalCuts = new float[0];

    private void Awake() => CreateBoard();
    
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
        wall.Clear();
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
            
            BoardObject obj = wall;
            if (i > 0)
            {
                if (components[i - 1] == null)
                    components[i - 1] = new BoardComponent("Component " + i, new Vector3(x, y));
                obj = components[i - 1];
            }
            
            AddCube(obj, x, y, 0f);
        }
    }
    
    private void AddCube(BoardObject obj, int x, int y, float z)
    {
        Vector3 a = GetVertexPosition(x, y, z);
        Vector3 b = GetVertexPosition(x, y + 1, z);
        Vector3 c = GetVertexPosition(x + 1, y, z);
        Vector3 d = GetVertexPosition(x + 1, y + 1, z);
        
        Vector3 e = GetVertexPosition(x, y, z + boardDepth);
        Vector3 f = GetVertexPosition(x, y + 1, z + boardDepth);
        Vector3 g = GetVertexPosition(x + 1, y, z + boardDepth);
        Vector3 h = GetVertexPosition(x + 1, y + 1, z + boardDepth);
        
        // front
        obj.AddFace(a, b, c, d);
        
        // left
        if (IsFaceVisible(x, y, -1, 0))
            obj.AddFace(a, e, b, f);
        
        // top
        if (IsFaceVisible(x, y, 0, 1))
            obj.AddFace(b, f, d, h);
        
        // right
        if (IsFaceVisible(x, y, 1, 0))
            obj.AddFace(d, h, c, g);
        
        // bottom
        if (IsFaceVisible(x, y, 0, -1))
            obj.AddFace(c, g, a, e);
    }

    private Vector3 GetVertexPosition(int x, int y, float z)
    {
        return new Vector3(
            x == 0 ? 0f : x <= horizontalCutAmount ? horizontalCuts[x - 1] : boardWidth,
            y == 0 ? 0f : y <= verticalCutAmount ? verticalCuts[y - 1] : boardHeight,
            z
        );
    }

    private bool IsFaceVisible(int x, int y, int dirX, int dirY)
    {
        int toX = x + dirX;
        int toY = y + dirY;
        
        if (toX < 0 || toX > horizontalCutAmount || toY < 0 || toY > verticalCutAmount)
            return true;

        return grid[x, y] != grid[toX, toY];
    }
    
    private void BuildMeshes()
    {
        wall.CreateMesh(wallMaterial, false);
        
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
                break;
            
            components[i].CreateObject(componentsParent);
            components[i].CreateMesh(componentMaterials[i % componentMaterials.Length], i > 0);
        }
    }

    private void OnValidate()
    {
        boardWidth = _boardWidth = Mathf.Max(0.01f, _boardWidth);
        boardHeight = _boardHeight = Mathf.Max(0.01f, _boardHeight);
        boardDepth = Mathf.Max(0.01f, boardDepth);
        
        horizontalCutAmount = _horizontalCutAmount = Mathf.Max(0, _horizontalCutAmount);
        verticalCutAmount = _verticalCutAmount = Mathf.Max(0, _verticalCutAmount);
    }
}
