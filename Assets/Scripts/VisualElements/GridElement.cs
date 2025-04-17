using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class GridElement : VisualElement
{
    [UxmlAttribute, CreateProperty]
    public int Columns
    {
        get => _Columns; set
        {
            _Columns = value;
            CreateItems();

        }
    }

    [UxmlAttribute, CreateProperty]
    public int Rows { get => _Rows; set { _Rows = value; CreateItems(); } }

    [UxmlAttribute, CreateProperty]
    public VisualTreeAsset ItemTemplate { get => _ItemTemplate; set { _ItemTemplate = value; CreateItems(); } }

    [SerializeField, DontCreateProperty]
    private int _Columns = 1;

    [SerializeField, DontCreateProperty]
    private int _Rows = 1;

    [SerializeField, DontCreateProperty]
    private VisualTreeAsset _ItemTemplate;

    public GridElement()
    {
    }


    private void CreateItems()
    {
        contentContainer.Clear();

        if (_Rows < 1 || _Columns < 1 || ItemTemplate == null)
            return;

        for (int i = 0; i < _Columns * _Rows; i++)
        {
            var element = ItemTemplate.Instantiate();
            hierarchy.Add(element);

        }

        UpdateChildren();
    }

    private void UpdateChildren()
    {
        var elements = hierarchy.Children();
        float size = Mathf.Min(contentRect.width / _Columns, contentRect.height / _Rows);

        int index = 0;
        for (int i = 0; i < _Columns; i++)
        {
            for (int j = 0; j < _Rows; j++, index++)
            {
                var element = elements.ElementAt(index);
                element.style.position = Position.Absolute;
                element.style.top = j * size;
                element.style.left = i * size;
                element.style.width = size;
                element.style.height = size;
            }

        }
    }
}
