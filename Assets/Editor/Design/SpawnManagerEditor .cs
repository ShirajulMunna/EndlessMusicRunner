using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnManager))]
public class SpawnManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpawnManager spawnManager = (SpawnManager)target;
        if (GUILayout.Button("Start Spawning Objects"))
        {
            spawnManager.StartSpawningObjectsEditor();
        }
        
        if (GUILayout.Button("Reset"))
        {
            spawnManager.ResetSpawning();
        }
    }
}
