using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace NodeSystem
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public UnityAction<Type, Vector2> OnCreateNodeEvent;

        private Type m_SupportedType;

        private Texture2D m_IndentionIcon;

        public void Initialize(Type supportedType)
        {
            m_SupportedType = supportedType;

            m_IndentionIcon = new Texture2D(1, 1);
            m_IndentionIcon.SetPixel(0, 0, Color.clear);
            m_IndentionIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"))

            };

            var types = TypeCache.GetTypesDerivedFrom(m_SupportedType);
            foreach (var type in types)
            {
                entries.Add(new SearchTreeEntry(new GUIContent(type.Name, m_IndentionIcon)) { level = 1, userData = type });
            }

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var data = SearchTreeEntry.userData as Type;

            OnCreateNodeEvent?.Invoke(data, context.screenMousePosition);

            return true;
        }
    }
}
