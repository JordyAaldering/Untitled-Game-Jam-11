using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName="Game Settings/Cut Settings", fileName="New Cut Settings")]
    public class CutSettings : ScriptableObject
    {
        [HideInInspector] public float width = 0;
        [HideInInspector] public float height = 0;
        
        [Range(0f, 1f)] public float maxCutOffset = 0.25f;
    }
}
