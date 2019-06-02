using System.Collections.Generic;
using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    public interface IComponent
    {
        void Measure(Size size, IRenderingContext context);
        void Arrange(Rectangle area);

        SizeUnit Width { get; }
        SizeUnit Height { get; }

        Size DesiredSize { get; }
        Rectangle Layout { get; }
        IEnumerable<IComponent> GetChildren();
    }
}