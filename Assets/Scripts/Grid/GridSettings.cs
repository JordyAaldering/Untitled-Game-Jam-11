using Board;
using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(menuName = "Game Settings/Grid Settings", fileName = "New Grid Settings")]
    public class GridSettings : ScriptableObject
    {
        [HideInInspector] public int[,] grid = new int[0, 0];

        [HideInInspector] public int width = 0;
        [HideInInspector] public int height = 0;
        
        public int DeadZoneSize => Mathf.CeilToInt((width + height) * 0.5f * deadZonePct);
        public int MinX => DeadZoneSize;
        public int MaxX => width - DeadZoneSize - 1;
        public int MinY => DeadZoneSize;
        public int MaxY => height - DeadZoneSize - 1;

        [Range(0f, 1f)] public float minRangePct = 0.25f;
        [Range(0f, 1f)] public float maxRangePct = 0.75f;
        [Range(0f, 1f)] public float stepStopChance = 0.5f;

        public int MinRange => Mathf.RoundToInt((width + height) * 0.5f * minRangePct);
        public int MaxRange => Mathf.RoundToInt((width + height) * 0.5f * maxRangePct);

        [Range(0f, 1f)] public float minComponentsPct = 0.25f;
        [Range(0f, 1f)] public float maxComponentsPct = 0.75f;
        [Range(0f, 1f)] public float componentStopChance = 0.5f;

        public int MinComponents => Mathf.CeilToInt((width + height) * 0.5f * minComponentsPct);
        public int MaxComponents => Mathf.CeilToInt((width + height) * 0.5f * maxComponentsPct);

        [Range(0f, 1f)] public float deadZonePct = 0.01f;
        public int maxFindTries = 10;

        public void Clear(BoardSettings boardSettings)
        {
            width = boardSettings.horizontalCutAmount + 1;
            height = boardSettings.verticalCutAmount + 1;
            grid = new int[width, height];
        }
    }
}
