using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialoqueEditor : GraphViewEditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private DialoqueTreeView m_TreeView;

    private DialoqueNodeList m_NodeListView;

    private DialogueInspectorView m_InspectorView;

    private DialoqueTree m_DialoqueTree;

    private SerializedObject m_SerializedObject;

    [MenuItem("Window/DialoqueEditor")]
    public static void ShowExample()
    {
        DialoqueEditor wnd = GetWindow<DialoqueEditor>();
        wnd.titleContent = new GUIContent("DialoqueEditor");
    }

    [OnOpenAsset]
    public static bool OpenDialoqueTreeAsset(int instanceID, int line)
    {
        var _object = EditorUtility.InstanceIDToObject(instanceID);
        if (_object is DialoqueTree _dialoque && _dialoque != null)
        {
            DialoqueEditor wnd = GetWindow<DialoqueEditor>();
            wnd.titleContent = new GUIContent(_object.name);
            wnd.Initialize(_dialoque);
            return true;
        }

        return false;
    }

    public void Initialize(DialoqueTree dialoqueTree)
    {
        m_DialoqueTree = dialoqueTree;
        m_SerializedObject = new SerializedObject(m_DialoqueTree);

        m_TreeView.PopulateGraph(m_DialoqueTree);

    }

    public void CreateGUI()
    {
        m_TreeView = new DialoqueTreeView();
        m_TreeView.style.flexGrow = 1;

        m_NodeListView = new DialoqueNodeList();
        InitializeNodeList(m_NodeListView);

        m_InspectorView = new DialogueInspectorView();
        m_InspectorView.style.flexGrow = 1;

        //setup selection callaback
        m_TreeView.OnNodeSelectedEvent += m_InspectorView.UpdateSelection;

        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        root.style.flexDirection = FlexDirection.Row;

        var main_view = new TwoPaneSplitView();
        main_view.style.flexGrow = 1;
        main_view.orientation = TwoPaneSplitViewOrientation.Horizontal;

        var left_view = new TwoPaneSplitView();
        left_view.orientation = TwoPaneSplitViewOrientation.Vertical;
        left_view.Add(m_NodeListView);
        left_view.Add(m_InspectorView);

        main_view.Add(left_view);
        main_view.Add(m_TreeView);


        root.Add(main_view);


        var stylesheet = Resources.Load<DialoqueSettings>("DialoqueSettings");
        root.styleSheets.Add(stylesheet.StyleSheet);

    }

    private void InitializeNodeList(DialoqueNodeList list)
    {
        list.style.flexGrow = 1;
        list.OnItemSelected += AddNewNode;
    }

    private void AddNewNode(Type type)
    {
        m_TreeView.CreateNode(type, Vector2.zero);
    }
}
