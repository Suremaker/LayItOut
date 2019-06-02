using System;
using System.Drawing;
using LayItOut.Components;
using LayItOut.Rendering;

namespace LayItOut
{
    public class Form
    {
        public IComponent Content { get; }

        public Form(IComponent content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public void LayOut(Size size, IRenderingContext context)
        {
            Content.Measure(size, context);
            Content.Arrange(new Rectangle(Point.Empty, size));
        }
    }
}
