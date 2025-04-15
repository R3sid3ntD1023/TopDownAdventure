using UnityEngine;

public class QuestGiverTrigger : QuestGiver
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        IAcceptQuest acceptee = null;
        if (other.gameObject.TryGetComponent(out acceptee))
        {
            if (acceptee != null)
            {
                if (!acceptee.QuestManager.HasQuest(Quest))
                    GiveQuest(acceptee);
            }
        }
    }
}
