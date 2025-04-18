using System;
using UnityEditor.Experimental.GraphView;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class PinAttribute : Attribute
{
    public Type InputType { get; private set; }
    public Port.Capacity Capacity { get; private set; }
    public Orientation Orientation { get; private set; }

    public PinAttribute(Type type, Port.Capacity capcity, Orientation orientation)
    {
        InputType = type;
        Capacity = capcity;
        Orientation = orientation;
    }
}

public class InputPinAttribute : PinAttribute
{
    public InputPinAttribute(Type type,
        Port.Capacity capacity = Port.Capacity.Single,
        Orientation orientation = Orientation.Horizontal)
        : base(type, capacity, orientation)
    {
    }
}

public class OutputPinAttribute : PinAttribute
{
    public OutputPinAttribute(Type type,
        Port.Capacity capacity = Port.Capacity.Single,
        Orientation orientation = Orientation.Horizontal)
        : base(type, capacity, orientation)
    {
    }
}
