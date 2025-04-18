using UnityEngine;

[RequireComponent(typeof(Dialoque))]
public class QuestGiver : MonoBehaviour, IInteractable
{

    private Dialoque m_Dialoque;

    public void Start()
    {
        m_Dialoque = GetComponent<Dialoque>();

    }

    void IInteractable.OnInteract(Interactee interactee)
    {
        if (interactee.GetComponent<IAcceptQuest>() is var acceptee && acceptee != null)
        {
            var blackboard = m_Dialoque.GetTree().Blackboard;
            if (blackboard)
            {
                blackboard.AddKey<IAcceptQuest>("Interactee");
                blackboard.SetKey("Interactee", acceptee);
            }

            m_Dialoque.Speak();

        }
    }
}

