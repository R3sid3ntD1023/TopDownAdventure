using UnityEngine;

public class FoodItem : MonoBehaviour, ItemInterface, IInteractable
{
    public InventoryItem InventoryItem;

    public int StackSize = 1;

    public InventoryItem GetInventoryItem()
    {
        return InventoryItem;
    }

    public int GetStackSize()
    {
        return StackSize;
    }

    public void RemoveFromStack(int amount)
    {
        StackSize -= amount;
        if (StackSize <= 0)
            Destroy(gameObject);
    }

    void IInteractable.OnInteract(Interactee interactee)
    {
        if (interactee.GetComponent<Inventory>() is var inventory)
        {
            inventory.AddItem(this);
        }
    }
}
