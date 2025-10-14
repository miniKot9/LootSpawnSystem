using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(LootSpace))]
public class LootSpaceOnEditor : Editor
{
    LootSpace script;
    GameObject scriptObject;

    SerializedProperty _lootPoints;
    SerializedProperty _itemsObjectsToSpawn;
    SerializedProperty _spawnOnStart;

    void OnEnable()
    {
        script = (LootSpace)target;
        scriptObject = script.gameObject;

        _lootPoints = serializedObject.FindProperty("_lootPoints");
        _itemsObjectsToSpawn = serializedObject.FindProperty("_itemsObjectsToSpawn");
        _spawnOnStart = serializedObject.FindProperty("_spawnOnStart");
    }

    public override void OnInspectorGUI()
    {
        //base.DrawDefaultInspector();

        serializedObject.Update();

        GUILayout.Label("Loot Space");

        GUILayout.BeginHorizontal("", "box");
        EditorGUILayout.PropertyField(_lootPoints);

        if (GUILayout.Button("Create new loot spawn point"))
        {
            script.CreateLootPoint();
            Debug.Log("Point created");
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(_itemsObjectsToSpawn);

        GUILayout.BeginHorizontal("Testing", "window");
        if (GUILayout.Button("Test Spawn"))
        {
            script.SummonLoot();
            Debug.Log("Spawned");
        }
        if (GUILayout.Button("Clear Objects"))
        {
            script.ClearLoot();
            Debug.Log("Cleared Objects");
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(_spawnOnStart);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
