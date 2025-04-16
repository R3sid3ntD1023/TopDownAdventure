using UnityEngine;
using UnityEngine.UIElements;


public partial class InventoryHandler : MonoBehaviour
{
    [SerializeField]
    private Inventory Inventory;

    [SerializeField]
    private UIDocument Document;

    private VisualElement InventoryElement;

    public VisualTreeAsset InventoryItemTemplate;

    private void Start()
    {
        if (Inventory == null)
            return;

        Document = GetComponent<UIDocument>();

        InventoryElement = Document.rootVisualElement.Q<VisualElement>("Inventory");
        Inventory.OnInventoryItemAdded.AddListener(OnInventoryItemAdded);
    }

    void OnInventoryItemAdded(InventoryItem item, int index)
    {
        if (InventoryElement == null || InventoryItemTemplate == null)
            return;

        var element = InventoryItemTemplate.CloneTree();
        element.dataSource = Inventory.Items[index];
        InventoryElement.Add(element);
    }

}
