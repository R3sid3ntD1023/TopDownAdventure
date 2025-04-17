using UnityEngine;
using UnityEngine.UIElements;

public class DialoqueManager : MonoBehaviour
{
    public static DialoqueManager Instance;

    private DialoqueManager() { }

    private DialoqueTree _currentDialoqueTree;

    private VisualElement RootElement;

    public void Awake()
    {
        if (Instance != null || Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
        var _dialoqueUI = GetComponent<UIDocument>();
        RootElement = _dialoqueUI.rootVisualElement;
        RootElement.Q<Button>("Submit").clicked += Execute;
    }

    public void SetCurrentTree(DialoqueTree tree)
    {
        if (_currentDialoqueTree == tree)
            return;

        _currentDialoqueTree = tree;
        _currentDialoqueTree.OnDialoqueTreeFinished.AddListener(OnTreeFinished);
        _currentDialoqueTree.OnDialoqueExecuted.AddListener(OnDialogChanged);
    }

    private void OnTreeFinished(DialoqueTree tree)
    {
        _currentDialoqueTree.OnDialoqueTreeFinished.RemoveListener(OnTreeFinished);
        _currentDialoqueTree = null;
    }

    private void OnDialogChanged(DialoqueNode node)
    {
        RootElement.dataSource = node;
    }

    private void Execute()
    {
        if (_currentDialoqueTree != null)
            _currentDialoqueTree.Execute();
    }
}
