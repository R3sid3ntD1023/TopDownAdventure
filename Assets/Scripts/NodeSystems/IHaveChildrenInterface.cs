using System.Collections.Generic;

namespace NodeSystem
{
    public interface IHaveChildrenInterface<T>
    {
        public abstract void AddChild(T child);

        public abstract void RemoveChild(T child);

        public abstract T GetChild(int index);

        public abstract bool HasChild(int index);

        public abstract List<T> GetChildren();
    }
}
