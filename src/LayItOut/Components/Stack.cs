using System;
using System.Collections.Generic;
using System.Drawing;

namespace LayItOut.Components
{
    public class Stack : Component, IContainer
    {
        private readonly List<IComponent> _children = new List<IComponent>();
        public void AddComponent(IComponent child)
        {
            _children.Add(child);
        }

        public override IEnumerable<IComponent> GetChildren() => _children;

        protected override Size OnMeasure(Size size)
        {
            var result = Size.Empty;
            foreach (var component in _children)
            {
                component.Measure(size);
                result.Width = Math.Max(result.Width, component.DesiredSize.Width);
                result.Height = Math.Max(result.Height, component.DesiredSize.Height);
            }

            return result;
        }

        protected override void OnArrange()
        {
            foreach (var child in _children)
                child.Arrange(Layout);
        }
    }
}