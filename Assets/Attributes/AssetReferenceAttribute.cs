using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
[Conditional("UNITY_EDITOR")]
public class AssetReferenceAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(AssetReferenceAttribute))]
public class AssetReferenceDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue != null)
        {
            EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(GameObject), false);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
