using UnityEngine;

public abstract class QuestGiver : MonoBehaviour
{
    public Quest Quest;
    private Quest _QuestInstance;

    public void Start()
    {
        if (Quest)
            _QuestInstance = Instantiate(Quest);
    }

    public void GiveQuest(IAcceptQuest acceptee)
    {
        if (_QuestInstance == null)
            return;

        if (!acceptee.QuestManager.HasQuest(_QuestInstance))
        {
            acceptee.QuestManager.ActivateQuest(_QuestInstance);
        }
    }
}

