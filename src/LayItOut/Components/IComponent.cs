﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    [Description("Basic interface describing any component.")]
    public interface IComponent
    {
        void Measure(Size size, IRendererContext context);
        void Arrange(Rectangle area);

        SizeUnit Width { get; }
        SizeUnit Height { get; }

        Size DesiredSize { get; }
        Rectangle Layout { get; }
        IEnumerable<IComponent> GetChildren();
    }
}