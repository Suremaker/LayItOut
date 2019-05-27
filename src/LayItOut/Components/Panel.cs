﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LayItOut.Components
{
    public class Panel : Component, IWrappingComponent
    {
        public IComponent Inner { get; set; }
        public Spacer Margin { get; set; }
        public Spacer Padding { get; set; }
        public Border Border { get; set; }
        public Color BackgroundColor { get; set; }
        public Rectangle BorderLayout { get; private set; }
        public Rectangle PaddingLayout { get; private set; }
        public Spacer ActualBorder => new Spacer(PaddingLayout.Top - BorderLayout.Top, PaddingLayout.Left - BorderLayout.Left, BorderLayout.Bottom - PaddingLayout.Bottom, BorderLayout.Right - PaddingLayout.Right);

        public override IEnumerable<IComponent> GetChildren()
        {
            if (Inner != null)
                yield return Inner;
        }

        protected override Size OnMeasure(Size size)
        {
            var panelSize = Margin.GetAbsoluteSize() + Padding.GetAbsoluteSize() + Border.AsSpacer().GetAbsoluteSize();

            Inner?.Measure((size - panelSize).Union(Size.Empty));

            return (Inner?.DesiredSize ?? Size.Empty) + panelSize;
        }

        protected override void OnArrange()
        {
            var (widthExpandable, heightExpandable) = WithExpandable(Margin, Border.AsSpacer(), Padding);
            var widths = widthExpandable.AsEnumerable().GetEnumerator();
            var heights = heightExpandable.AsEnumerable().GetEnumerator();

            BorderLayout = Layout.ShrinkBy(GetRealSize(Margin, widths, heights));
            PaddingLayout = BorderLayout.ShrinkBy(GetRealSize(Border.AsSpacer(), widths, heights));
            Inner?.Arrange(PaddingLayout.ShrinkBy(GetRealSize(Padding, widths, heights)));
        }

        private (int[] widths, int[] heights) WithExpandable(params Spacer[] spacers)
        {
            var total = Inner?.DesiredSize ?? Size.Empty;
            var widthExpandable = Inner?.Width.IsUnlimited ?? false ? 1 : 0;
            var heightExpandable = Inner?.Height.IsUnlimited ?? false ? 1 : 0;

            foreach (var s in spacers)
            {
                if (s.Left.IsUnlimited) ++widthExpandable;
                if (s.Right.IsUnlimited) ++widthExpandable;
                if (s.Top.IsUnlimited) ++heightExpandable;
                if (s.Bottom.IsUnlimited) ++heightExpandable;
                total += s.GetAbsoluteSize();
            }

            var remaining = Layout.Size - total;
            return (SizeUnit.Distribute(remaining.Width, widthExpandable),
                SizeUnit.Distribute(remaining.Height, heightExpandable));
        }

        private Spacer GetRealSize(Spacer spacer, IEnumerator<int> widths, IEnumerator<int> heights)
        {
            var l = spacer.Left.GetValue(widths.GetNext);
            var r = spacer.Right.GetValue(widths.GetNext);
            var t = spacer.Top.GetValue(heights.GetNext);
            var b = spacer.Bottom.GetValue(heights.GetNext);
            return new Spacer(t, l, b, r);
        }
    }
}
