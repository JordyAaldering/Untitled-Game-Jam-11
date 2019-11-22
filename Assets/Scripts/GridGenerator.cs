using UnityEngine;

namespace DefaultNamespace
{
    public static class GridGenerator
    {
        public static int[,] PopulateGrid(GridSettings settings)
        {
            int[,] grid = new int[settings.width, settings.height];
            
            int componentIndex = 1;
            for (int x = settings.MinX; x < settings.MaxX; x++)
            for (int y = settings.MinY; y < settings.MaxY; y++)
            {
                if (grid[x, y] != 0)
                    continue;
                
                CreateComponent(ref grid, x, y, componentIndex, settings);
                componentIndex++;
                
                if (componentIndex > settings.maxComponents)
                    return grid;
            }
            
            return grid;
        }

        private static void CreateComponent(ref int[,] grid, int x, int y, int index, GridSettings settings)
        {
            int steps = 0;
            while (steps < settings.maxSteps)
            {
                grid[x, y] = index;
                
                int dirX = Random.Range(x > settings.MinX ? -1 : 0, x < settings.MaxX ? 2 : 1);

                if (dirX == 0) 
                    y += Random.Range(y > settings.MinY ? -1 : 0, y < settings.MaxY ? 2 : 1);
                else
                    x += dirX;

                steps++;
            }
        }
    }

    [CreateAssetMenu(menuName="Grid Settings", fileName="New Grid Settings")]
    public class GridSettings : ScriptableObject
    {
        [HideInInspector] public int width = 0;
        [HideInInspector] public int height = 0;
        
        public int deadZoneSize = 1;
        public int maxSteps = 1;
        public int maxComponents = 1;

        public int MinX => deadZoneSize;
        public int MaxX => width - deadZoneSize - 1;
        public int MinY => deadZoneSize;
        public int MaxY => height - deadZoneSize - 1;
        
        private void OnValidate()
        {
            deadZoneSize = Mathf.Max(0, deadZoneSize);
            maxSteps = Mathf.Max(0, maxSteps);
            maxComponents = Mathf.Max(1, maxComponents);
        }
    }
}
