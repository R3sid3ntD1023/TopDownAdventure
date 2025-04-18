using UnityEngine;
using UnityEngine.UIElements;

public class DialoqueManager : MonoBehaviour
{
    public static DialoqueManager Instance;

    public UIDocument DialoqueUI;

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

    }

    public void SetCurrentTree(DialoqueTree tree)
    {
        if (_currentDialoqueTree == tree)
            return;

        _currentDialoqueTree = tree;
        _currentDialoqueTree.OnFinished += OnTreeFinished;

        DialoqueUI.gameObject.SetActive(true);
        RootElement = DialoqueUI.rootVisualElement;
        RootElement.Q<Button>("Submit").clicked += Execute;
        RootElement.dataSource = tree;
    }

    private void OnTreeFinished(DialoqueTree tree)
    {
        _currentDialoqueTree.OnFinished -= OnTreeFinished;
        _currentDialoqueTree = null;
        RootElement.dataSource = null;

        DialoqueUI.gameObject.SetActive(false);
    }

    private void Execute()
    {
        if (_currentDialoqueTree != null)
            _currentDialoqueTree.Execute();
    }
}
