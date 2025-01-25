using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GazeGuidingPathPlayer))]
public class GazeGuidingPathPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GazeGuidingPathPlayer script = (GazeGuidingPathPlayer)target;

        // Draw the default inspector, excluding specific fields
        DrawPropertiesExcluding(serializedObject, "DirectionCueFadeDuration", "pathDisplayDistance", "animationDuration", "lineMaterial");

        // Add space and headline for Direction Cue settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Direction Cue Settings", EditorStyles.boldLabel);

        // Disable the DirectionCueFadeDuration field if DirectionCueEnabled is false
        EditorGUI.BeginDisabledGroup(!script.DirectionCueEnabled);
        script.DirectionCueFadeDuration = EditorGUILayout.FloatField("Direction Cue Fade Duration", script.DirectionCueFadeDuration);
        EditorGUI.EndDisabledGroup();

        // Add space and headline for Direction Arrow settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Direction Arrow Settings", EditorStyles.boldLabel);

        // Disable the pathDisplayDistance, animationDuration, and lineMaterial fields if DirectionArrowEnabled is false
        EditorGUI.BeginDisabledGroup(!script.DirectionArrowEnabled);
        script.pathDisplayDistance = EditorGUILayout.FloatField("Path Display Distance", script.pathDisplayDistance);
        script.animationDuration = EditorGUILayout.FloatField("Animation Duration", script.animationDuration);
        script.lineMaterial = (Material)EditorGUILayout.ObjectField("Line Material", script.lineMaterial, typeof(Material), true);
        EditorGUI.EndDisabledGroup();

        // Add space and headline for actions
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

        // Add buttons for Clear Line and Trigger TEST
        if (GUILayout.Button("Clear Line"))
        {
            script.ClearLine();
        }

        if (GUILayout.Button("Trigger TEST"))
        {
            script.triggerTEST();
        }

        if (GUILayout.Button("Trigger TEST2"))
        {
            script.triggerTEST2(true);
        }
        if (GUILayout.Button("Trigger TEST3"))
        {
            script.triggerTEST2(false);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}