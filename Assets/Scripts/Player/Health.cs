using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float CurrentHealth;

    public float MaxHealth;


    public UnityEvent OnDeath;

    public bool Heal(float amount)
    {
        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth += Mathf.Clamp(amount, 0, MaxHealth - CurrentHealth);
            return true;
        }

        return false;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= Mathf.Clamp(damage, 0, CurrentHealth);

            if (CurrentHealth == 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}
