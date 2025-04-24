using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnInventoryItemAddedEvent : UnityEvent<InventoryItem, int> { }
public class OnInventoryItemUpdatedEvent : UnityEvent<InventoryItem, int> { }

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> Items;

    public OnInventoryItemAddedEvent OnInventoryItemAdded = new OnInventoryItemAddedEvent();

    [SerializeField]
    private InventoryItem m_CurrentItem;

    [SerializeField]
    private int m_CurrentItemIndex = -1;


    public void Start()
    {

    }

    public void AddItem(ItemInterface item)
    {

        AddItem(item, item.GetStackSize());
    }

    public void AddToExistingItems(ItemInterface item, ref int remainder)
    {
        var inventory_item = item.GetInventoryItem();
        var items = Items.FindAll(i => i != null && i.ID == inventory_item.ID && i.CurrentStackSize != inventory_item.MaxStackSize);
        if (items.Count == 0)
            return;

        foreach (var _item in items)
        {
            if (remainder <= 0)
                return;

            int removed = Mathf.Clamp(remainder, 0, Mathf.Abs(_item.MaxStackSize - _item.CurrentStackSize));
            _item.CurrentStackSize += removed;
            remainder -= removed;
            item.RemoveFromStack(removed);
        }

    }

    public void AddItem(ItemInterface item, int stack)
    {
        int remainder = stack;

        var inventory_item = item.GetInventoryItem();
        while (remainder > 0)
        {

            AddToExistingItems(item, ref remainder);

            int added = Mathf.Clamp(remainder, 0, inventory_item.MaxStackSize);

            if (added > 0)
            {
                var instance = Instantiate(inventory_item);


                var slot_index = Items.FindIndex(i => i == null);

                if (slot_index != -1)
                {
                    item.RemoveFromStack(added);

                    instance.CurrentStackSize = added;
                    instance.OnEmpty += RemoveItem;

                    Items[slot_index] = instance;
                    OnInventoryItemAdded.Invoke(instance, slot_index);

                    m_CurrentItem = instance;
                    m_CurrentItemIndex = slot_index;
                }
            }

            remainder -= added;

        }
    }

    public void RemoveItem(InventoryItem item)
    {
        var index = Items.IndexOf(item);
        if (Items.Remove(item))
        {

            if (m_CurrentItem == item)
            {
                m_CurrentItemIndex = index - 1;
                m_CurrentItem = m_CurrentItemIndex > 0 ? Items[m_CurrentItemIndex] : null;

            }
        }
    }

    public void EquipNext()
    {
        if (Items.Count == 0)
            return;

        m_CurrentItemIndex = (m_CurrentItemIndex + 1) % Items.Count;
        m_CurrentItem = Items[m_CurrentItemIndex];
    }

    public void EquipPrevious()
    {
        if (Items.Count == 0)
            return;

        m_CurrentItemIndex = (m_CurrentItemIndex - 1) % Items.Count;
        m_CurrentItem = Items[m_CurrentItemIndex];
    }

    public void Use()
    {
        m_CurrentItem?.Use(this.gameObject);
    }
}
