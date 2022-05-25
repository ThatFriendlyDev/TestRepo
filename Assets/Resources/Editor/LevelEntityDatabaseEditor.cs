using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelEntityDatabase))]
public class LevelEntityDatabaseEditor : Editor
{
    SerializedProperty entities;

    private void OnEnable()
    {
        this.entities = this.serializedObject.FindProperty("entities");
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        LevelEntityDatabase database = (LevelEntityDatabase)target;

        if (GUILayout.Button("Generate ID's"))
        {
            var cacheEntities = FindObjectsOfType<MonoBehaviour>(true).OfType<ICacheEntity>();

            foreach (ICacheEntity entity in cacheEntities)
            {
                if (database.Has(entity.GetGameObject()))
                {
                    continue;
                }

                database.entities.Add(new LevelEntityId
                {
                    go = entity.GetGameObject(),
                    uniqueId = System.Guid.NewGuid().ToString(),
                });
            }
        }

        if (GUILayout.Button("Regenerate ID's (WARNING!)"))
        {
            var cacheEntities = FindObjectsOfType<MonoBehaviour>(true).OfType<ICacheEntity>();
            database.entities.Clear();

            foreach (ICacheEntity entity in cacheEntities)
            {
                database.entities.Add(new LevelEntityId
                {
                    go = entity.GetGameObject(),
                    uniqueId = System.Guid.NewGuid().ToString(),
                });
            }
        }

        if (GUILayout.Button("Check Warnings"))
        {
            List<string> values = new List<string>();

            foreach (LevelEntityId entity in database.entities)
            {
                if (entity.go == null)
                {
                    Debug.Log("NULL GameObject for key: " + entity.uniqueId);
                    continue;
                }

                if (values.Contains(entity.uniqueId))
                {
                    Debug.Log("Duplicated ID: " + entity.uniqueId + ", Name: " + entity.go.name);
                    continue;
                }

                values.Add(entity.uniqueId);
            }
        }

        if (GUILayout.Button("Clear"))
        {
            database.entities.Clear();
        }

        EditorGUILayout.PropertyField(this.entities);

        if (GUI.changed)
        {
            this.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(this.target);
        }
    }
}
