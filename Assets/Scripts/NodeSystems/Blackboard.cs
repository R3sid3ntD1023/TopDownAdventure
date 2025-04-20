using System.Collections.Generic;
using UnityEngine;



namespace NodeSystem
{

    [System.Serializable]
    public class BlackboardKeyBase
    {
        public string Name;

        private object Value;

        public T GetValue<T>() where T : class
        {
            if (Value as T is var v && v != null)
                return v;

            return null;
        }

        public void SetValue<T>(T value) { Value = value; }

        public BlackboardKeyBase(string name)
        {
            Name = name;
        }
    }


    [CreateAssetMenu(fileName = "BlackBoard", menuName = "Node/BlackBoard")]
    public class Blackboard : ScriptableObject
    {
        public List<BlackboardKeyBase> Keys = new List<BlackboardKeyBase>();

        public Blackboard()
        {
            Keys.Add(new BlackboardKeyBase("Self"));
        }

        public void AddKey(string name)
        {
            if (HasKey(name))
                return;

            Keys.Add(new BlackboardKeyBase(name));
        }

        public BlackboardKeyBase GetKey(string name)
        {
            return Keys.Find(x => x.Name == name);
        }

        public void SetKey<T>(string name, T value)
        {
            var key = GetKey(name);
            if (key != null)
                key.SetValue(value);
        }

        public bool HasKey(string name)
        {
            return Keys.Exists(x => x.Name == name);
        }
    }
}

