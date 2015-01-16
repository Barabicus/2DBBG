using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Chunk))]

public class ChunkInspector : Editor
{
    SerializedProperty blockSize;
    SerializedProperty chunkSize;
    SerializedProperty useThreading;
    SerializedProperty tickTime;
    SerializedProperty generator;
    SerializedProperty updateMethod;
    SerializedProperty useColliders;

  //  SerializedProperty autoGenerateOnStart;

    void OnEnable()
    {

        blockSize = serializedObject.FindProperty("blockSize");
        chunkSize = serializedObject.FindProperty("chunkSize");
        useThreading = serializedObject.FindProperty("useThreading");
        tickTime = serializedObject.FindProperty("tickTime");
        generator = serializedObject.FindProperty("generator");
        updateMethod = serializedObject.FindProperty("updateMethod");
        useColliders = serializedObject.FindProperty("useColliders");

   //     autoGenerateOnStart = serializedObject.FindProperty("autoGenerateOnStart");

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        //EditorGUILayout.PropertyField(blockSize, new GUIContent("Block Size", "The size each block should be"));
        //EditorGUILayout.PropertyField(chunkSize, new GUIContent("Chunk Size", "How many blocks horizontally and vertically each chunk should contain. "));
        //EditorGUILayout.PropertyField(useThreading, new GUIContent("Use Threading", "When possible should the chunk try to use threadin to save time"));
        //EditorGUILayout.PropertyField(tickTime, new GUIContent("Tick Time", "How many often the chunk will tick. Each chunk tick will trigger a tick event in any blocks that implement ITickable"));
        //EditorGUILayout.PropertyField(generator, new GUIContent("Chunk Generator", "The Generator that will be used to construct the chunk"));
        //EditorGUILayout.PropertyField(updateMethod, new GUIContent("Update Method", "The update method the chunk will use to update"));
        //EditorGUILayout.PropertyField(useColliders, new GUIContent("Use Colliders", "If the chunk should create a block collider for every block active."));



    //    EditorGUILayout.PropertyField(autoGenerateOnStart, new GUIContent("Generate On Chunk Start", "If the chunk does not depend on the world this will allow the chunk to load itself. This should not be enabled if the chunk initilization is being controlled elsewhere"));

        if (GUILayout.Button(new GUIContent("Update Colliders")))
        {
            Chunk chunk = (Chunk)target;
            chunk.SetupBoxColliders();
        }

        if (GUILayout.Button(new GUIContent("Force Update")))
        {
            Chunk chunk = (Chunk)target;
            chunk.DrawAllBlocks();
        }


        serializedObject.ApplyModifiedProperties();

    }

}
