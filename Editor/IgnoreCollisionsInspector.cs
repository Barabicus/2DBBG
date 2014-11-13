using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(IgnoreCollisions))]
public class IgnoreCollisionsInspector : Editor
{

    SerializedProperty ignoreList;
    int newLayer1;
    int newLayer2;

    void Start()
    {
        ignoreList = serializedObject.FindProperty("ignoreList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.HelpBox("Ignore 2D Physics Layers", MessageType.None);

        EditorGUILayout.HelpBox("Add 2D Physics Layer", MessageType.None);


        EditorGUILayout.BeginHorizontal();

        int v1, v2;
        int.TryParse(EditorGUILayout.TextField(newLayer1 + ""), out v1);
        int.TryParse(EditorGUILayout.TextField(newLayer2 + ""), out v2);

        newLayer1 = v1;
        newLayer2 = v2;


        EditorGUILayout.EndHorizontal();

        GUILayout.Button("Add Layer");

        serializedObject.ApplyModifiedProperties();

        DrawDefaultInspector();
    }
}
