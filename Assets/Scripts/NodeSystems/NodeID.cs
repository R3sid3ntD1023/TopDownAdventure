
using System;
using UnityEngine;

[System.Serializable]
public class NodeID
{
    [SerializeField, ReadOnlyProperty]
    private string _id;

    public string ID { get { return _id; } }

    NodeID()
    {
        _id = Guid.NewGuid().ToString();
    }
}
