using System;
using System.Drawing;
using LayItOut.Components;

namespace LayItOut.Tests.Components.TestHelpers
{
    class TestableComponent : Component
    {
        public Action<Rectangle> OnArrangeCallback = _ => { };
        public Func<Size, Size> OnMeasureCallback = size => size;

        protected override Size OnMeasure(Size size) => OnMeasureCallback(size);
        protected override void OnArrange() => OnArrangeCallback(Layout);
    }
}