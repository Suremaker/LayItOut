using System.Collections.Generic;

namespace LayItOut.Components
{
    public abstract class Container : Component, IContainer
    {
        private readonly List<IComponent> _children = new List<IComponent>();
        public void AddComponent(IComponent child)
        {
            _children.Add(child);
        }

        public override IEnumerable<IComponent> GetChildren() => _children;
    }
}