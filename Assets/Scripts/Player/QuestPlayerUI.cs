using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class QuestPlayerUI : MonoBehaviour
{
    public QuestPlayer Acceptee;

    private VisualElement Root;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {

        if (Acceptee == null)
            return;

        var quest_manager = Acceptee.GetQuestManager();

        Root = gameObject.GetComponent<UIDocument>().rootVisualElement;

        var active = Root.Q<ListView>("ActiveQuests");
        var completed = Root.Q<ListView>("CompletedQuests");

        active.itemsSource = quest_manager.ActiveQuests;
        active.bindItem = (VisualElement element, int index) =>
            {
                element.dataSource = quest_manager.ActiveQuests[index];
            };

        completed.itemsSource = quest_manager.CompletedQuests;
        completed.bindItem = (VisualElement element, int index) =>
        {
            element.dataSource = quest_manager.CompletedQuests[index];
        };
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if (Root == null || !context.performed) return;

        var element = Root.Q<VisualElement>("UI");

        if (!element.ClassListContains("toggable"))
        {
            element.AddToClassList("toggable");
        }
        else
        {
            element.RemoveFromClassList("toggable");
        }
    }
}
