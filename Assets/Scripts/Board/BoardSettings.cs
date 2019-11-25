using UnityEngine;

namespace Board
{
    [CreateAssetMenu(menuName = "Game Settings/Board Settings", fileName = "New Board Settings")]
    public class BoardSettings : ScriptableObject
    {
        [Header("Transform")]
        public float boardWidth = 16f;
        public float boardHeight = 10f;
        public float boardDepth = 1f;
    
        [Header("Grid")]
        public int horizontalCutAmount = 16;
        public int verticalCutAmount = 10;
    
        [Header("Mesh")]
        public Material wallMaterial;
        public Material[] componentMaterials;

        public Material GetComponentMaterial(int i) => componentMaterials[i % componentMaterials.Length];
    
        private void OnValidate()
        {
            boardWidth = Mathf.Max(0.01f, boardWidth);
            boardHeight = Mathf.Max(0.01f, boardHeight);
            boardDepth = Mathf.Max(0.01f, boardDepth);
        
            horizontalCutAmount = Mathf.Max(0, horizontalCutAmount);
            verticalCutAmount = Mathf.Max(0, verticalCutAmount);
        }
    }
}
