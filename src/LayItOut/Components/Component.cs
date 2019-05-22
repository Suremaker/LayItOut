using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LayItOut.Components
{
    public class Component : IComponent
    {
        public void Measure(Size size)
        {
            DesiredSize = OnMeasure(size);
        }

        public void Arrange(Rectangle area)
        {
            Layout = area;
            OnArrange();
        }

        public SizeUnit Width { get; set; }
        public SizeUnit Height { get; set; }

        protected virtual void OnArrange() { }
        protected virtual Size OnMeasure(Size size) => Size.Empty.ApplyIfSet(Width, Height);

        public Size DesiredSize { get; private set; }
        public Rectangle Layout { get; private set; }
        public virtual IEnumerable<IComponent> GetChildren() => Enumerable.Empty<IComponent>();
    }
}
