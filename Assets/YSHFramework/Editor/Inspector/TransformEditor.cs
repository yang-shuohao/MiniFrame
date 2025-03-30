using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    private bool unifyPosition = false;
    private bool unifyRotation = false;
    private bool unifyScale = false;

    public override void OnInspectorGUI()
    {
        Transform transform = (Transform)target;

        // Position
        EditorGUILayout.BeginHorizontal();
        Vector3 newPosition = EditorGUILayout.Vector3Field("Position", transform.position);
        unifyPosition = EditorGUILayout.Toggle(unifyPosition, GUILayout.Width(20));
        if (GUILayout.Button("Reset", GUILayout.Width(50)))
        {
            newPosition = Vector3.zero;
        }
        EditorGUILayout.EndHorizontal();

        if (unifyPosition)
        {
            newPosition.y = newPosition.z = newPosition.x;
        }

        // Apply Position
        if (transform.position != newPosition)
        {
            Undo.RecordObject(transform, "Change Position");
            transform.position = newPosition;
        }

        // Rotation
        EditorGUILayout.BeginHorizontal();
        Vector3 newRotation = EditorGUILayout.Vector3Field("Rotation", transform.eulerAngles);
        unifyRotation = EditorGUILayout.Toggle(unifyRotation, GUILayout.Width(20));
        if (GUILayout.Button("Reset", GUILayout.Width(50)))
        {
            newRotation = Vector3.zero;
        }
        EditorGUILayout.EndHorizontal();

        if (unifyRotation)
        {
            newRotation.y = newRotation.z = newRotation.x;
        }

        // Apply Rotation
        if (transform.eulerAngles != newRotation)
        {
            Undo.RecordObject(transform, "Change Rotation");
            transform.eulerAngles = newRotation;
        }

        // Scale
        EditorGUILayout.BeginHorizontal();
        Vector3 newScale = EditorGUILayout.Vector3Field("Scale", transform.localScale);
        unifyScale = EditorGUILayout.Toggle(unifyScale, GUILayout.Width(20));
        if (GUILayout.Button("Reset", GUILayout.Width(50)))
        {
            newScale = Vector3.one;
        }
        EditorGUILayout.EndHorizontal();

        if (unifyScale)
        {
            newScale.y = newScale.z = newScale.x;
        }

        // Apply Scale
        if (transform.localScale != newScale)
        {
            Undo.RecordObject(transform, "Change Scale");
            transform.localScale = newScale;
        }
    }
}
