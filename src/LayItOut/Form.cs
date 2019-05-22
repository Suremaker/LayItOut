using System;
using System.Drawing;
using LayItOut.Components;

namespace LayItOut
{
    public class Form
    {
        public IComponent Content { get; }

        public Form(IComponent content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public void LayOut(Size size)
        {
            Content.Measure(size);
            Content.Arrange(new Rectangle(Point.Empty, size));
        }
    }
}
