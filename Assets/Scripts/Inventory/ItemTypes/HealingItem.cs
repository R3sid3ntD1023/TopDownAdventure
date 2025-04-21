using UnityEngine;

public class HealingItem : InventoryItem
{
    public float HealAmount;

    public override void Use(GameObject user)
    {
        base.Use(user);

        if (user.GetComponent<Health>() is var health)
        {
            if (!health.Heal(HealAmount))
                return;

            CurrentStackSize -= 1;

            if (CurrentStackSize == 0)
            {
                OnEmpty?.Invoke(this);
            }
        }
    }
}
