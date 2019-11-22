using UnityEngine;

namespace DefaultNamespace
{
    public static class GridGenerator
    {
        public static int[,] PopulateGrid(GridSettings settings)
        {
            int[,] grid = new int[settings.width, settings.height];
            
            int components = 0;
            int x = (settings.MinX + settings.MaxX) / 2;
            int y = (settings.MinY + settings.MaxY) / 2;
            while (components < settings.maxComponents)
            {
                if (components > settings.maxComponents && Random.Range(0f, 1f) < settings.componentStopChance)
                    return grid;

                components++;
                CreateComponent(ref grid, ref x, ref y, components, settings);
                
                int tries = 0;
                while (grid[x, y] != 0)
                {
                    int dirX = Random.Range(x > settings.MinX ? -1 : 0, x < settings.MaxX ? 2 : 1);
                    if (dirX == 0) y += Random.Range(y > settings.MinY ? -1 : 0, y < settings.MaxY ? 2 : 1);
                    else x += dirX;
                    
                    tries++;
                    if (tries > settings.nextStartMaxTries)
                        return grid;
                }
            }
            
            return grid;
        }

        private static void CreateComponent(ref int[,] grid, ref int x, ref int y, int index, GridSettings settings)
        {
            int steps = 0;
            while (steps < settings.maxSteps)
            {
                if (steps > settings.maxSteps && Random.Range(0f, 1f) < settings.stepStopChance)
                    return;
                
                grid[x, y] = index;
                
                int dirX = Random.Range(x > settings.MinX ? -1 : 0, x < settings.MaxX ? 2 : 1);
                if (dirX == 0) y += Random.Range(y > settings.MinY ? -1 : 0, y < settings.MaxY ? 2 : 1);
                else x += dirX;

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
        
        public int minSteps = 1;
        public int maxSteps = 2;
        [Range(0f, 1f)] public float stepStopChance = 0.5f;
        
        public int minComponents = 1;
        public int maxComponents = 2;
        [Range(0f, 1f)] public float componentStopChance = 0.5f;

        public int nextStartMaxTries = 10;
        
        public int MinX => deadZoneSize;
        public int MaxX => width - deadZoneSize - 1;
        public int MinY => deadZoneSize;
        public int MaxY => height - deadZoneSize - 1;
        
        private void OnValidate()
        {
            deadZoneSize = Mathf.Max(0, deadZoneSize);
            
            maxSteps = Mathf.Max(0, maxSteps);
            minSteps = Mathf.Clamp(minSteps, 0, maxSteps);

            maxComponents = Mathf.Max(1, maxComponents);
            minComponents = Mathf.Clamp(minComponents, 1, maxComponents);

            nextStartMaxTries = Mathf.Max(1, nextStartMaxTries);
        }
    }
}
