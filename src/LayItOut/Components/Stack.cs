using System;
using System.ComponentModel;
using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    [Description("A container that will lay-out it's children one on top of the other, where the last one would be on top.\n\nWhen measured, it will take a size to fit all it's children.")]
    public class Stack : Container
    {
        protected override Size OnMeasure(Size size, IRendererContext context)
        {
            var result = Size.Empty;
            foreach (var component in GetChildren())
            {
                component.Measure(size, context);
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