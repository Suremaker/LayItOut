using System;
using System.Drawing;
using LayItOut.Components;
using LayItOut.Rendering;

namespace LayItOut.Tests.TestHelpers
{
    class TestableComponent : Component
    {
        public Action<Rectangle> OnArrangeCallback = _ => { };
        public Func<Size, Size> OnMeasureCallback = size => size;

        protected override Size OnMeasure(Size size, IRendererContext context) => OnMeasureCallback(size);
        protected override void OnArrange() => OnArrangeCallback(Layout);
    }
}