using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeSystem
{

    public class BlackboardEditor : EditorWindow
    {
        private Blackboard m_BlackBoard;

        private BlackboardView m_BlackboardView;

        [MenuItem("Window/UI Toolkit/BlackboardEditor")]
        public static void ShowExample()
        {
            BlackboardEditor wnd = GetWindow<BlackboardEditor>();
            wnd.titleContent = new GUIContent("BlackboardEditor");
        }

        [OnOpenAsset]
        public static bool OpenBlackboardAsset(int instanceID, int line)
        {
            var _object = EditorUtility.InstanceIDToObject(instanceID);
            if (_object is Blackboard blackboard && blackboard != null)
            {
                BlackboardEditor wnd = GetWindow<BlackboardEditor>();
                wnd.titleContent = new GUIContent(_object.name);
                wnd.Initialize(blackboard);
                return true;
            }

            return false;
        }

        public void Initialize(Blackboard blackboard)
        {
            m_BlackboardView?.Initialize(blackboard);
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            m_BlackboardView = new BlackboardView();

            root.Add(m_BlackboardView);
        }
    }
}
