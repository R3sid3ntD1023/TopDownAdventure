using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    [ReadOnlyProperty]
    public string ID;

    public string Name = "Name";

    public string Description = "Description";

    public Texture2D Icon;

    public int MaxStackSize = 1;

    [AssetReference]
    public GameObject WorldPrefab;

    [ReadOnlyProperty]
    public int CurrentStackSize = 0;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(ID))
            ID = GUID.Generate().ToString();
    }
}
