using System.Collections.Generic;
using System.Drawing;

namespace LayItOut.Components
{
    public interface IComponent
    {
        void Measure(Size size);
        void Arrange(Rectangle area);

        SizeUnit Width { get; }
        SizeUnit Height { get; }

        Size DesiredSize { get; }
        Rectangle Layout { get; }
        IEnumerable<IComponent> GetChildren();
    }
}