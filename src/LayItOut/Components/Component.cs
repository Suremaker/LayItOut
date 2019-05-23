using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LayItOut.Components
{
    public class Component : IComponent
    {
        public SizeUnit Width { get; set; }
        public SizeUnit Height { get; set; }
        public Alignment Alignment { get; set; }
        public Size DesiredSize { get; private set; }
        public Rectangle Layout { get; private set; }

        public void Measure(Size size)
        {
            DesiredSize = OnMeasure(size.ApplyIfSet(Width, Height)).ApplyIfSet(Width, Height);
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
        protected virtual Size OnMeasure(Size size) => Size.Empty;

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
