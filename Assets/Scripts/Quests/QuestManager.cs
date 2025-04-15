using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnReceiveRewardEvent : UnityEvent<QuestReward> { }

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Manager")]
public class QuestManager : ScriptableObject
{
    public List<Quest> ActiveQuests;

    public List<Quest> CompletedQuests;

    public OnReceiveRewardEvent OnReceiveReward = new OnReceiveRewardEvent();

    public void ActivateQuest(Quest quest)
    {
        ActiveQuests.Add(quest);

        Debug.Log("Accepted Quest");
    }

    public void CompleteQuest(Quest quest)
    {
        ActiveQuests.Remove(quest);
        CompletedQuests.Add(quest);
        OnReceiveReward.Invoke(quest.Information.Reward);

        Debug.Log("Completed Quest");
    }

    public bool HasQuest(Quest quest)
    {
        return ActiveQuests.Contains(quest) && !CompletedQuests.Contains(quest);
    }
}
