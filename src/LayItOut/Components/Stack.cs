using System;
using System.Drawing;

namespace LayItOut.Components
{
    public class Stack : Container
    {
        protected override Size OnMeasure(Size size)
        {
            var result = Size.Empty;
            foreach (var component in GetChildren())
            {
                component.Measure(size);
                result.Width = Math.Max(result.Width, component.DesiredSize.Width);
                result.Height = Math.Max(result.Height, component.DesiredSize.Height);
            }

            return result;
        }

        protected override void OnArrange()
        {
            foreach (var child in GetChildren())
                child.Arrange(Layout);
        }
    }
}