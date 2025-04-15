using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class QuestPlayerUI : MonoBehaviour
{
    public QuestPlayer Acceptee;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        if (Acceptee == null)
            return;

        var quest_manager = Acceptee.GetQuestManager();

        var root = gameObject.GetComponent<UIDocument>().rootVisualElement;

        var active = root.Q<ListView>("ActiveQuests");
        var completed = root.Q<ListView>("CompletedQuests");

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
}
