using CustomAttributes;
using NodeSystem;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialoqueSystem
{
    public class QuestNode : DialoqueBaseNode
    {
        [AssetReference]
        public Quest Quest;

        private Quest m_QuestInstance = null;

        public override VisualElement CreateInspectorGUI()
        {
            var field = new PropertyField();
            field.bindingPath = "Quest";
            return field;
        }

        protected override void OnBeginExecute()
        {
            if (Quest != null && !m_QuestInstance)
                m_QuestInstance = Instantiate(Quest);
        }

        protected override ENodeState OnExecute()
        {
            if (m_QuestInstance == null)
                return ENodeState.Finished;

            var blackboard = GetBlackboard();
            var interactee = blackboard.GetKey("Interactee")?.Value as IAcceptQuest;
            if (interactee != null)
            {
                var manager = interactee.GetQuestManager();
                if (manager == null && manager.HasQuest(m_QuestInstance))
                    return ENodeState.Finished;

                manager.ActivateQuest(Instantiate(m_QuestInstance));
                Debug.Log($"Gave Quest to {interactee}");
            }

            return ENodeState.Finished;
        }
    }
}