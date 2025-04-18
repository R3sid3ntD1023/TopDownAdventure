using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BlackBoardKeyBase
{
    public string Name;

    public abstract object Value { get; set; }

    public BlackBoardKeyBase(string name)
    {
        Name = name;
    }
}

public class BlackBoardKey<T> : BlackBoardKeyBase
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

    public BlackBoardKey(string name) : base(name) { }
}

public class BlackBoardKeyGameObject : BlackBoardKey<GameObject>
{
    public BlackBoardKeyGameObject(string name) : base(name) { }
}

[CreateAssetMenu(fileName = "BlackBoard", menuName = "Node/BlackBoard")]
public class Blackboard : ScriptableObject
{
    [SerializeReference]
    public List<BlackBoardKeyBase> Keys = new List<BlackBoardKeyBase>();

    public Blackboard()
    {
        Keys.Add(new BlackBoardKeyGameObject("Self"));
    }

    public void AddKey<T>(string name)
    {
        if (HasKey(name))
            return;

        Keys.Add(new BlackBoardKey<T>(name));
    }

    public BlackBoardKeyBase GetKey(string name)
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
