using System;
using System.Drawing;
using LayItOut.Components;
using LayItOut.Rendering;

namespace LayItOut
{
    public class Form
    {
        public IComponent Content { get; private set; }

        public Form(IComponent content)
        {
            UpdateContent(content);
        }

        public void UpdateContent(IComponent content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public void LayOut(Size size, IRendererContext context)
        {
            Content.Measure(size, context);
            Content.Arrange(new Rectangle(Point.Empty, size));
        }
    }
}
