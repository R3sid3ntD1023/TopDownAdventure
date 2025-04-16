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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
