using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GazeGuidingPathPlayer))]
public class GazeGuidingPathPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GazeGuidingPathPlayer script = (GazeGuidingPathPlayer)target;
        if (GUILayout.Button("Clear Line"))
        {
            script.ClearLine();
        }

        if (GUILayout.Button("Trigger TEST"))
        {
            script.triggerTEST();
        }
    }
}