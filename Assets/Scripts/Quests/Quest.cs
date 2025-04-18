using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum QuestState
{
    Pending = 0,
    Active = 1,
    Completed = 2,
}

public class OnGoalCompletedEvent : UnityEvent { }

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    [System.Serializable]
    public struct Info
    {
        public string Name;

        public string Description;

        public QuestReward Reward;

    }

    public abstract class QuestGoal : ScriptableObject
    {
        protected string Description;

        public int CurrentAmount { get; private set; }

        public int RequirementAmount = 1;

        public OnGoalCompletedEvent OnGoalCompleted { get; private set; }

        public virtual string GetDscription()
        {
            return Description;
        }

        public void Evaluate()
        {
            if (CurrentAmount >= RequirementAmount)
            {
                OnGoalCompleted.Invoke();
            }
        }
    }

    [Header("Info")] public Info Information;

    public List<QuestGoal> Goals;

    private QuestState State;

    public bool IsCompleted() { return State != QuestState.Active; }

    public void Activate(IAcceptQuest acceptee)
    {
        State = QuestState.Active;
        acceptee.GetQuestManager().ActivateQuest(this);
    }

    public void Complete(IAcceptQuest acceptee)
    {
        State = QuestState.Completed;
        acceptee.GetQuestManager().CompleteQuest(this);
    }
}
