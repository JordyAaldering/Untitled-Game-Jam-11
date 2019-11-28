using System;
using Board;
using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(menuName = "Game Settings/Grid Settings", fileName = "New Grid Settings")]
    public class GridSettings : ScriptableObject
    {
        [HideInInspector] public int difficulty = 0;
        [HideInInspector] public int width = 0;
        [HideInInspector] public int height = 0;
        [HideInInspector] public GridPoint[,] grid = new GridPoint[0, 0];
        
        public int deadZoneSize = 1;
        public int MinX => deadZoneSize;
        public int MaxX => width - deadZoneSize - 1;
        public int MinY => deadZoneSize;
        public int MaxY => height - deadZoneSize - 1;

        [SerializeField] private int _minRange = 1;
        [SerializeField] private int _maxRange = 3;
        [Range(0f, 1f)] public float stepStopChance = 0.5f;
        public int minRange => _minRange + difficulty / 3;
        public int maxRange => _maxRange + difficulty / 3;

        [SerializeField] private int _minComponents = 1;
        [SerializeField] private int _maxComponents = 3;
        [Range(0f, 1f)] public float componentStopChance = 0.5f;
        public int minComponents => _minComponents + difficulty;
        public int maxComponents => _maxComponents + difficulty;
        
        public void Clear(BoardSettings boardSettings)
        {
            width = boardSettings.horizontalCutAmount + 1;
            height = boardSettings.verticalCutAmount + 1;
            
            grid = new GridPoint[width, height];
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new GridPoint(0);
            }
        }
        
        public Texture2D GetTexture()
        {
            Texture2D tex = new Texture2D(width, height)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            
            Color[] colors = new Color[width * height];
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                colors[x + y * width] = grid[x, y].value == 0 ? new Color(0f, 0f, 0f, 0f) : Color.white;
            }

            tex.SetPixels(colors);
            tex.Apply();
            return tex;
        }
    }
}
