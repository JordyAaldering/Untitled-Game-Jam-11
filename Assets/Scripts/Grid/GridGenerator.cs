using Board;
using UnityEngine;

namespace Grid
{
    public static class GridGenerator
    {
        public static void PopulateGrid(BoardSettings boardSettings, GridSettings gridSettings)
        {
            gridSettings.width = boardSettings.horizontalCutAmount + 1;
            gridSettings.height = boardSettings.verticalCutAmount + 1;
            gridSettings.grid = new int[gridSettings.width, gridSettings.height];

            int components = 0;
            int x = (gridSettings.MinX + gridSettings.MaxX) / 2;
            int y = (gridSettings.MinY + gridSettings.MaxY) / 2;
            while (components < gridSettings.MaxComponents)
            {
                if (components > gridSettings.MinComponents && Random.Range(0f, 1f) < gridSettings.componentStopChance)
                    return;

                components++;
                CreateComponent(ref x, ref y, components, gridSettings);

                int tries = 0;
                while (gridSettings.grid[x, y] != 0)
                {
                    int dirX = Random.Range(x > gridSettings.MinX ? -1 : 0, x < gridSettings.MaxX ? 2 : 1);
                    if (dirX == 0) y += Random.Range(y > gridSettings.MinY ? -1 : 0, y < gridSettings.MaxY ? 2 : 1);
                    else x += dirX;

                    tries++;
                    if (tries > gridSettings.maxFindTries)
                        return;
                }
            }
        }

        private static void CreateComponent(ref int x, ref int y, int i, GridSettings gridSettings)
        {
            int steps = 0, tries = 0;
            while (steps < gridSettings.MaxRange)
            {
                if (steps > gridSettings.MinRange && Random.Range(0f, 1f) < gridSettings.stepStopChance)
                    return;

                if (gridSettings.grid[x, y] == 0)
                {
                    gridSettings.grid[x, y] = i;
                    steps++;
                }
                else
                {
                    tries++;
                    if (tries > gridSettings.maxFindTries)
                        return;
                }

                int dirX = Random.Range(x > gridSettings.MinX ? -1 : 0, x < gridSettings.MaxX ? 2 : 1);
                if (dirX == 0) y += Random.Range(y > gridSettings.MinY ? -1 : 0, y < gridSettings.MaxY ? 2 : 1);
                else x += dirX;
            }
        }
    }
}
