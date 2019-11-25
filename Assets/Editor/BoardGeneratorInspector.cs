using UnityEditor;
using UnityEngine;

namespace Board
{
    [CustomEditor(typeof(BoardGenerator))]
    public class BoardGeneratorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("Generate"))
            {
                BoardGenerator boardGenerator = (BoardGenerator) target;
                boardGenerator.Generate();
            }
        }
    }
}
