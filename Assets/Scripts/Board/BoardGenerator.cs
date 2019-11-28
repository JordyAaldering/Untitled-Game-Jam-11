#pragma warning disable 0649
using Board.Object;
using Cut;
using Grid;
using UnityEngine;

namespace Board
{
    public class BoardGenerator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private BoardSettings boardSettings;
        [SerializeField] private CutSettings cutSettings;
        [SerializeField] private GridSettings gridSettings;
        [SerializeField] private GridCurrent gridCurrent;
    
        [Header("References")]
        [SerializeField] private GameObject wallObject;
        [SerializeField] private Transform componentsParent;
        
        private BoardWall _wall;
        private BoardWall wall => _wall ?? (_wall = new BoardWall(wallObject));
        private BoardComponent[] components = new BoardComponent[0];

        public void Generate() => CreateBoard();
    
        private void CreateBoard()
        {
            Clear();
        
            CutGenerator.Cut(boardSettings, cutSettings);
            GridGenerator.PopulateGrid(boardSettings, gridSettings);

            CreateMeshes();
            BuildMeshes();
        }
    
        private void Clear()
        {
            wall.Clear();
            gridSettings.Clear(boardSettings);

            int childCount = componentsParent.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(componentsParent.GetChild(i).gameObject);
            }
        }
    
        private void CreateMeshes()
        {
            for (int x = 0; x <= boardSettings.horizontalCutAmount; x++)
            for (int y = 0; y <= boardSettings.verticalCutAmount; y++)
            {
                if (gridSettings.grid[x, y].value == 0)
                    AddCube(wall, x, y, 0f);
            }
            
            gridCurrent.Populate(gridSettings);
            gridCurrent.Shuffle(gridSettings);
            
            components = new BoardComponent[gridCurrent.components.Count];
            foreach (var c in gridCurrent.components)
            {
                int i = c.Key;
                if (i == 0)
                    continue;
                
                components[i - 1] = new BoardComponent("Component " + i, GetVertexPosition(c.Value[0].x, c.Value[0].y, 0f));
                foreach (var v in c.Value)
                {
                    AddCube(components[i - 1], v.x, v.y, 0f);
                }
            }
        }
    
        private void AddCube(BoardObject obj, int x, int y, float z)
        {
            Vector3 a = GetVertexPosition(x, y, z);
            Vector3 b = GetVertexPosition(x, y + 1, z);
            Vector3 c = GetVertexPosition(x + 1, y, z);
            Vector3 d = GetVertexPosition(x + 1, y + 1, z);
        
            // front
            obj.AddFace(a, b, c, d);
        
            Vector3 e = GetVertexPosition(x, y, z + boardSettings.boardDepth);
            Vector3 f = GetVertexPosition(x, y + 1, z + boardSettings.boardDepth);
            Vector3 g = GetVertexPosition(x + 1, y, z + boardSettings.boardDepth);
            Vector3 h = GetVertexPosition(x + 1, y + 1, z + boardSettings.boardDepth);

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
                x == 0 ? 0f : x <= boardSettings.horizontalCutAmount ? cutSettings.horizontalCuts[x - 1] : boardSettings.boardWidth,
                y == 0 ? 0f : y <= boardSettings.verticalCutAmount ? cutSettings.verticalCuts[y - 1] : boardSettings.boardHeight,
                z
            );
        }

        private bool IsFaceVisible(int x, int y, int dirX, int dirY)
        {
            int toX = x + dirX, toY = y + dirY;
            if (toX < 0 || toX > boardSettings.horizontalCutAmount || toY < 0 || toY > boardSettings.verticalCutAmount)
                return true;

            return gridSettings.grid[x, y] != gridSettings.grid[toX, toY];
        }
    
        private void BuildMeshes()
        {
            wall.CreateMesh(boardSettings.wallMaterial);
            foreach (BoardComponent c in components)
            {
                if (c != null)
                {
                    c.CreateObject(componentsParent);
                    c.CreateMesh(boardSettings.componentMaterial);
                }
            }
        }
    }
}
