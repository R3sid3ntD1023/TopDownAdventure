using UnityEngine;

public class FoodItem : MonoBehaviour, ItemInterface
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
}
