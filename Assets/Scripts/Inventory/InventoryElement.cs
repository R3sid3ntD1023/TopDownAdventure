using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class InventoryElement : VisualElement
{
    [SerializeField, DontCreateProperty]
    private VisualTreeAsset _item_template;

    [UxmlAttribute, CreateProperty]
    public VisualTreeAsset ItemTemplate
    {
        get { return _item_template; }
        set { _item_template = value; }
    }


    public InventoryElement()
    {

    }
}
