using System.Drawing;
using LayItOut.Components;
using LayItOut.Rendering;

namespace LayItOut.Tests.TestHelpers
{
    class FixedMeasureComponent : Component
    {
        private readonly Size _desiredSize;
        public FixedMeasureComponent(int desiredWidth, int desiredHeight) : this(new Size(desiredWidth, desiredHeight)) { }
        public FixedMeasureComponent(Size desiredSize)
        {
            _desiredSize = desiredSize;
        }

        protected override Size OnMeasure(Size size, IRenderingContext context) => _desiredSize;
    }
}