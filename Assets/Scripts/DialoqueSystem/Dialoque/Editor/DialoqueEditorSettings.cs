using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialoqueSystem
{
    public class DialoqueSettings : ScriptableObject
    {
        public const string SettingsPath = "Assets/Resources/DialoqueSettings.asset";

        public StyleSheet StyleSheet;

        public VisualTreeAsset NodeElementUxml;

        public static DialoqueSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<DialoqueSettings>(SettingsPath);

            if (settings == null)
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
                settings = ScriptableObject.CreateInstance<DialoqueSettings>();
                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }

    }

    class DialoqueSettingsProvider : SettingsProvider
    {
        private SerializedObject m_Settings;

        public const string SettingsPath = "Assets/Resources/DialoqueSettings.asset";

        public DialoqueSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope)
        {

        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            m_Settings = DialoqueSettings.GetSerializedSettings();
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.PropertyField(m_Settings.FindProperty("StyleSheet"));
            EditorGUILayout.PropertyField(m_Settings.FindProperty("NodeElementUxml"));
            m_Settings.ApplyModifiedPropertiesWithoutUndo();
        }

        [SettingsProvider]
        public static SettingsProvider CreateDialoqueSettingsProvider()
        {
            return new DialoqueSettingsProvider("Project/Dialoque", SettingsScope.Project);
        }
    }
}