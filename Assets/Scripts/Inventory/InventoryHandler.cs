using System.Linq;
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

        for (int i = 0; i < Inventory.Items.Count; i++)
        {
            var element = InventoryItemTemplate.CloneTree();
            InventoryElement.Add(element);
        }
    }

    void OnInventoryItemAdded(InventoryItem item, int index)
    {
        if (InventoryElement == null || InventoryItemTemplate == null)
            return;

        var children = InventoryElement.Children().ToList();

        children[index].dataSource = Inventory.Items[index];

    }

}
