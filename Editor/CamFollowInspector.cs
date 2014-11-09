using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CamFollow2D))]
public class CamFollowInspector : Editor
{

    SerializedProperty followX;
    SerializedProperty followY;
    SerializedProperty xDefaultPos;
    SerializedProperty yDefaultPos;

    SerializedProperty followTarget;

    void OnEnable()
    {
        followX = serializedObject.FindProperty("followX");
        followY = serializedObject.FindProperty("followY");
        xDefaultPos = serializedObject.FindProperty("xDefaultPos");
        yDefaultPos = serializedObject.FindProperty("yDefaultPos");
        followTarget = serializedObject.FindProperty("follow");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(followTarget, new GUIContent("Target To Follow", "The Target the camera should follow"));
        EditorGUILayout.PropertyField(followX, new GUIContent("Follow X Position", "Should The Camera follow the X position of the target, if not set the position X should be at"));
        if (!followX.boolValue)
            EditorGUILayout.PropertyField(xDefaultPos, new GUIContent("X Position", "The Position the Camera should set it's X position to"));
        EditorGUILayout.PropertyField(followY, new GUIContent("Follow Y Position", "Should The Camera follow the Y position of the target, if not set the position Y should be at"));
        if (!followY.boolValue)
            EditorGUILayout.PropertyField(yDefaultPos, new GUIContent("Y Position", "The Position the Camera should set it's Y position to"));

        serializedObject.ApplyModifiedProperties();
    }
}