using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using LayItOut.Attributes;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    [Description("A generic component that can have dimensions and specified alignment.")]
    public class Component : IComponent
    {
        [Description("Component width. If **not set**, the minimum measure size will be used. If set to **unlimited**, component will expand to take all free space. It can be set to **absolute** size as well.")]
        public SizeUnit Width { get; set; }
        [Description("Component height. If **not set**, the minimum measure size will be used. If set to **unlimited**, component will expand to take all free space. It can be set to **absolute** size as well.")]
        public SizeUnit Height { get; set; }
        [Description("Component alignment. Used when component is given more space to lay-out than it's actual dimensions.")]
        public Alignment Alignment { get; set; }
        public Size DesiredSize { get; private set; }
        public Rectangle Layout { get; private set; }

        public void Measure(Size size, IRendererContext context)
        {
            DesiredSize = OnMeasure(size.ApplyIfSet(Width, Height), context).ApplyIfSet(Width, Height);
        }

        public void Arrange(Rectangle area)
        {
            var actualSize = GetActualSize(area.Size);
            var alignment = Alignment.GetShift(area.Size - actualSize);
            Layout = new Rectangle(area.Location + alignment, actualSize);
            OnArrange();
        }

        public virtual IEnumerable<IComponent> GetChildren() => Enumerable.Empty<IComponent>();
        protected virtual void OnArrange() { }
        protected virtual Size OnMeasure(Size size, IRendererContext context) => Size.Empty;

        private Size GetActualSize(Size areaSize)
        {
            var actualSize = DesiredSize.Intersect(areaSize);
            if (Width == SizeUnit.Unlimited)
                actualSize.Width = areaSize.Width;
            if (Height == SizeUnit.Unlimited)
                actualSize.Height = areaSize.Height;
            return actualSize;
        }
    }
}
