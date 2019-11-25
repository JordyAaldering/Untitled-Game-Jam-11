using UnityEngine;

namespace Cut
{
    [CreateAssetMenu(menuName = "Game Settings/Cut Settings", fileName = "New Cut Settings")]
    public class CutSettings : ScriptableObject
    {
        [HideInInspector] public float[] horizontalCuts = new float[0];
        [HideInInspector] public float[] verticalCuts = new float[0];
        
        [HideInInspector] public float wallWidth = 0;
        [HideInInspector] public float wallHeight = 0;

        [Range(0f, 1f)] public float maxCutOffset = 0.25f;
    }
}
