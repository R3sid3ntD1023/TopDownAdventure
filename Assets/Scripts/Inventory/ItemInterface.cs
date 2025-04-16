public interface ItemInterface
{
    public InventoryItem GetInventoryItem();

    public int GetStackSize();

    public void RemoveFromStack(int amount);
}
