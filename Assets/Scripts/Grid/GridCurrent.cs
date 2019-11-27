using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Grid
{
    [CreateAssetMenu(menuName = "Game Settings/Grid Current", fileName = "New Grid Current")]
    public class GridCurrent : ScriptableObject
    {
        [HideInInspector] public int[,] grid = new int[0, 0];
        [HideInInspector] public Dictionary<int, List<Vector2Int>> components = new Dictionary<int, List<Vector2Int>>();

        private int width = 0;
        private int height = 0;
        
        public void Populate(GridSettings gridSettings)
        {
            width = gridSettings.width;
            height = gridSettings.height;
            grid = new int[width, height];
            
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                int i = gridSettings.grid[x, y];
                grid[x, y] = i;
                
                if (!components.ContainsKey(i) || components[i] == null)
                    components[i] = new List<Vector2Int>();
                components[i].Add(new Vector2Int(x, y));
            }
        }

        public bool TryMove(int i, Vector2Int dir)
        {
            foreach (Vector2Int v in components[i])
            {
                int x = v.x + dir.x;
                int y = v.y + dir.y;
                if (x < 0 || x >= width || y < 0 || y >= height)
                    return false;
                
                int j = grid[x, y];
                if (j != 0 && j != i)
                    return false;
            }

            foreach (Vector2Int v in components[i])
            {
                grid[v.x, v.y] = 0;
            }

            List<Vector2Int> next = new List<Vector2Int>();
            foreach (Vector2Int v in components[i])
            {
                int x = v.x + dir.x;
                int y = v.y + dir.y;
                grid[x, y] = i;
                next.Add(new Vector2Int(x, y));
            }
            
            components[i] = next;
            return true;
        }

        public bool TryRotate(int i)
        {
            foreach (Vector2Int v in components[i])
            {
                grid[v.x, v.y] = 0;
            }
            
            List<Vector2Int> next = new List<Vector2Int>();
            foreach (Vector2Int v in components[i])
            {
                Vector2Int r = v.Rotate90Around(components[i][0]);
                grid[r.x, r.y] = i;
                next.Add(r);
            }

            components[i] = next;
            return true;
        }

        public void Clear()
        {
            components.Clear();
        }
    }
}
