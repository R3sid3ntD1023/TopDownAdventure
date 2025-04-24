using CustomAttributes;
using NodeSystem;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialoqueSystem
{

    [System.Serializable]
    public struct DialoqueInfo
    {
        public string Speaker;

        [TextArea]
        public string Message;

        [AssetReference]
        public Sprite Sprite;
    }


    [CreateAssetMenu(fileName = "Dialoque", menuName = "Dialoque/DialoqueNode")]
    public class DialoqueNode : DialoqueBaseNode
    {
        public DialoqueInfo Info;


        protected override ENodeState OnExecute()
        {
            Debug.Log($"Speaker: {Info.Speaker} - {Info.Message}");

            return ENodeState.Success;
        }

        public override VisualElement CreateInspectorGUI()
        {
            PropertyField field = new PropertyField();
            field.bindingPath = "Info";

            VisualElement visualElement = new VisualElement();
            visualElement.Bind(new UnityEditor.SerializedObject(this));
            visualElement.Add(field);

            return visualElement;
        }
    }
}