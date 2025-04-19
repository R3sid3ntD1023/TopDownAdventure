using System.Collections.Generic;
using UnityEngine;



namespace NodeSystem
{
    using BlackboardKeyGameObject = BlackboardKey<GameObject>;

    [System.Serializable]
    public abstract class BlackboardKeyBase
    {
        public string Name;

        public abstract object Value { get; set; }

        public BlackboardKeyBase(string name)
        {
            Name = name;
        }
    }

    public class BlackboardKey<T> : BlackboardKeyBase
    {
        private T KeyValue;

        public override object Value
        {
            get => KeyValue; set
            {
                if (value is T)
                    KeyValue = (T)value;
            }
        }

        public BlackboardKey(string name) : base(name) { }
    }


    [CreateAssetMenu(fileName = "BlackBoard", menuName = "Node/BlackBoard")]
    public class Blackboard : ScriptableObject
    {
        [SerializeReference]
        public List<BlackboardKeyBase> Keys = new List<BlackboardKeyBase>();

        public Blackboard()
        {
            Keys.Add(new BlackboardKeyGameObject("Self"));
        }

        public void AddKey<T>(string name)
        {
            if (HasKey(name))
                return;

            Keys.Add(new BlackboardKey<T>(name));
        }

        public BlackboardKeyBase GetKey(string name)
        {
            return Keys.Find(x => x.Name == name);
        }

        public void SetKey<T>(string name, T value)
        {
            var key = GetKey(name);
            if (key != null)
                key.Value = value;
        }

        public bool HasKey(string name)
        {
            return Keys.Exists(x => x.Name == name);
        }
    }
}

