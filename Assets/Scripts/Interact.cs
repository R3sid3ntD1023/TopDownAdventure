using UnityEngine;
using UnityEngine.Events;

public class Interactee : MonoBehaviour
{
    public UnityEvent<IInteractable> OnInterableChanged;

    private IInteractable m_CurrentInterable = null;

    public void Interact()
    {
        if (m_CurrentInterable == null)
            return;

        m_CurrentInterable.Interact(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() is var interactable && interactable != m_CurrentInterable)
        {
            m_CurrentInterable = interactable;
            OnInterableChanged?.Invoke(m_CurrentInterable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() is var interactable && interactable == m_CurrentInterable)
        {
            m_CurrentInterable = null;
            OnInterableChanged?.Invoke(null);
        }
    }
}
