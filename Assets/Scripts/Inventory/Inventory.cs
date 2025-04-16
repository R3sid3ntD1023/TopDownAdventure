using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> Items;

    public void AddItem(ItemInterface item)
    {
        var inventory_item = item.GetInventoryItem();
        AddItem(inventory_item, item.GetStackSize());
    }

    public void AddToExistingItems(InventoryItem item, ref int remainder)
    {
        var items = Items.FindAll(i => i.ID == item.ID && item.CurrentStackSize != item.MaxStackSize);
        if (items.Count == 0)
            return;

        foreach (var _item in items)
        {
            if (remainder <= 0)
                return;

            int removed = Mathf.Clamp(remainder, 0, Mathf.Abs(_item.MaxStackSize - _item.CurrentStackSize));
            _item.CurrentStackSize += removed;
            remainder -= removed;
        }

    }

    public void AddItem(InventoryItem item, int stack)
    {
        int remainder = stack;

        AddToExistingItems(item, ref remainder);

        while (remainder > 0)
        {
            int added = Mathf.Clamp(remainder, 0, item.MaxStackSize);

            if (added > 0)
            {
                var instance = Instantiate(item);
                instance.CurrentStackSize = added;
                Items.Add(instance);
            }

            remainder -= added;

        }
    }

}
