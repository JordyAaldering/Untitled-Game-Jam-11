using System.Collections.Generic;
using Board;
using UnityEngine;
using Utilities;

namespace Grid
{
    public static class GridGenerator
    {
        public static void PopulateGrid(BoardSettings boardSettings, GridSettings gridSettings)
        {
            gridSettings.Clear(boardSettings);

            int components = 0;
            int startX = (gridSettings.MinX + gridSettings.MaxX) / 2;
            int startY = (gridSettings.MinY + gridSettings.MaxY) / 2;
            
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            gridSettings.grid[startX, startY] = new GridPoint(1) {depth = gridSettings.maxRange / 2};

            while (queue.Count > 0)
            {
                Vector2Int[] dirs =
                {
                    new Vector2Int(1, 0),
                    new Vector2Int(-1, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(0, -1)
                };
                dirs.Shuffle();
                
                Vector2Int curr = queue.Dequeue();
                foreach (Vector2Int dir in dirs)
                {
                    Vector2Int next = new Vector2Int(curr.x + dir.x, curr.y + dir.y);
                    GridPoint nextPoint = gridSettings.grid[next.x, next.y];
                    
                    if (next.x < gridSettings.MinX || next.x >= gridSettings.MaxX || next.y < gridSettings.MinY || next.y >= gridSettings.MaxY)
                        continue;
            
                    if (nextPoint.value != 0)
                        continue;

                    if (TryAddGridPoint(gridSettings, curr, next))
                    {
                        if (nextPoint.visits < 4)
                            queue.Enqueue(next);
                    }
                    else if (gridSettings.grid[next.x, next.y].value == 0 && TryAddComponent(gridSettings, ref components, next))
                    {
                        if (nextPoint.visits < 4)
                            queue.Enqueue(next);
                    }
                }
            }
        }

        private static bool TryAddGridPoint(GridSettings gridSettings, Vector2Int curr, Vector2Int next)
        {
            GridPoint currPoint = gridSettings.grid[curr.x, curr.y];
            GridPoint nextPoint = gridSettings.grid[next.x, next.y];
            nextPoint.visits++;

            if (currPoint.depth >= gridSettings.maxRange)
                return false;

            if (currPoint.depth >= gridSettings.minRange && Random.Range(0f, 1f) < gridSettings.stepStopChance)
                return false;
            
            nextPoint = new GridPoint(currPoint.value, nextPoint.visits + 1, currPoint);
            gridSettings.grid[next.x, next.y] = nextPoint;
            return true;
        }

        private static bool TryAddComponent(GridSettings gridSettings, ref int components, Vector2Int next)
        {
            GridPoint nextPoint = gridSettings.grid[next.x, next.y];
            nextPoint.visits++;

            if (components >= gridSettings.maxComponents)
                return false;
            
            if (components >= gridSettings.minComponents && Random.Range(0f, 1f) < gridSettings.componentStopChance)
                return true;
            
            components++;
            nextPoint = new GridPoint(components, nextPoint.visits + 1);
            gridSettings.grid[next.x, next.y] = nextPoint;
            return true;
        }
    }
}
