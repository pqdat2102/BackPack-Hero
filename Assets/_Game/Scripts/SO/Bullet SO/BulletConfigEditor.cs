using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletConfig))]
public class BulletConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BulletConfig config = (BulletConfig)target;

        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("movementType"));

        if (config.movementType == BulletConfig.MovementType.Curved)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Curved Movement Parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationSpeed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("curvedHeight"));
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Effect", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectDelay"));

        if (config.effectType == BulletEffectType.Explosion)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Explosion Effect Parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("childBulletConfig"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("childBulletCount"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("explosionRadius"));
        }
        else if (config.effectType == BulletEffectType.Area)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Area Effect Parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("areaEffectPrefab"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("areaDamage"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("areaLifetime"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}