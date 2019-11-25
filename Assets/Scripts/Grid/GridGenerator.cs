using UnityEngine;

namespace Grid
{
    public static class GridGenerator
    {
        public static int[,] PopulateGrid(GridSettings settings)
        {
            int[,] grid = new int[settings.width, settings.height];

            int components = 0;
            int x = (settings.MinX + settings.MaxX) / 2;
            int y = (settings.MinY + settings.MaxY) / 2;
            while (components < settings.MaxComponents)
            {
                if (components > settings.MinComponents && Random.Range(0f, 1f) < settings.componentStopChance)
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
                    if (tries > settings.maxFindTries)
                        return grid;
                }
            }

            return grid;
        }

        private static void CreateComponent(ref int[,] grid, ref int x, ref int y, int i, GridSettings settings)
        {
            int steps = 0, tries = 0;
            while (steps < settings.MaxRange)
            {
                if (steps > settings.MinRange && Random.Range(0f, 1f) < settings.stepStopChance)
                    return;

                if (grid[x, y] == 0)
                {
                    grid[x, y] = i;
                    steps++;
                }
                else
                {
                    tries++;
                    if (tries > settings.maxFindTries)
                        return;
                }

                int dirX = Random.Range(x > settings.MinX ? -1 : 0, x < settings.MaxX ? 2 : 1);
                if (dirX == 0) y += Random.Range(y > settings.MinY ? -1 : 0, y < settings.MaxY ? 2 : 1);
                else x += dirX;
            }
        }
    }
}
