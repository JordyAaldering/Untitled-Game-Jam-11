using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("Generate"))
            {
                GameManager boardGenerator = (GameManager) target;
                boardGenerator.StartLevel();
            }
        }
    }
}
