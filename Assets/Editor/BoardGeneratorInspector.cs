using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardGenerator))]
public class BoardGeneratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BoardGenerator boardGenerator = (BoardGenerator) target;

        if (GUILayout.Button("Generate"))
        {
            boardGenerator.Generate();
        }
    }
}
