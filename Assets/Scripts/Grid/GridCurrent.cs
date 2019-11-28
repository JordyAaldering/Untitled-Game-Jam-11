using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Grid
{
    [CreateAssetMenu(menuName = "Game Settings/Grid Current", fileName = "New Grid Current")]
    public class GridCurrent : ScriptableObject
    {
        [HideInInspector] public int[,] grid = new int[0, 0];
        [HideInInspector] public Dictionary<int, List<Vector2Int>> components = new Dictionary<int, List<Vector2Int>>();
        [HideInInspector] public int maxIndex = 1;
        
        private int width = 0;
        private int height = 0;
        
        public void Populate(GridSettings gridSettings)
        {
            components.Clear();
            width = gridSettings.width;
            height = gridSettings.height;
            grid = new int[width, height];
            maxIndex = 1;

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                int i = gridSettings.grid[x, y].value;
                grid[x, y] = i;
                maxIndex = Mathf.Max(maxIndex, i);
                
                if (!components.ContainsKey(i) || components[i] == null)
                    components[i] = new List<Vector2Int>();
                components[i].Add(new Vector2Int(x, y));
            }
        }

        public bool TryMove(GridSettings gridSettings, int i, Vector2Int dir, bool useDeadZone = false)
        {
            foreach (Vector2Int v in components[i])
            {
                int x = v.x + dir.x;
                int y = v.y + dir.y;
                if (useDeadZone && (x < gridSettings.MinX || x >= gridSettings.MaxX || y < gridSettings.MinY || y >= gridSettings.MaxY))
                    return false;
                else if (x < 0 || x >= width || y < 0 || y >= height)
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

        public bool TryRotate(GridSettings gridSettings, int i, bool useDeadZone = false)
        {
            foreach (var r in components[i].Select(v => v.Rotate90Around(components[i][0])))
            {
                if (useDeadZone && (r.x < gridSettings.MinX || r.x >= gridSettings.MaxX || r.y < gridSettings.MinY || r.y >= gridSettings.MaxY))
                    return false;
                else if (r.x < 0 || r.x >= width || r.y < 0 || r.y >= height)
                    return false;
                
                int j = grid[r.x, r.y];
                if (j != 0 && j != i)
                    return false;
            }
            
            foreach (Vector2Int v in components[i])
            {
                grid[v.x, v.y] = 0;
            }
            
            List<Vector2Int> next = new List<Vector2Int>();
            foreach (var r in components[i].Select(v => v.Rotate90Around(components[i][0])))
            {
                grid[r.x, r.y] = i;
                next.Add(r);
            }

            components[i] = next;
            return true;
        }

        public void Shuffle(GridSettings gridSettings)
        {
            Vector2Int[] dirs =
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            };
            
            for (int i = 0; i < 100; i++)
            {
                int comp = Random.Range(1, maxIndex);
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    Vector2Int dir = dirs[Random.Range(0, 4)];
                    TryMove(gridSettings, comp, dir, true);
                }
                else
                {
                    TryRotate(gridSettings, comp, true);
                }
            }
        }

        public bool CheckSolution(GridSettings gridSettings)
        {
            return components.Where(c => c.Key != 0).All(c => c.Value.All(v => gridSettings.grid[v.x, v.y].value != 0));
        }
    }
}
