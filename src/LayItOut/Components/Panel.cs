using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    public class Panel : Component, IWrappingComponent
    {
        public IComponent Inner { get; set; }
        public Spacer Margin { get; set; }
        public Spacer Padding { get; set; }
        public Border Border { get; set; }
        public BorderRadius BorderRadius { get; set; }
        public Color BackgroundColor { get; set; }
        public Rectangle BorderLayout { get; private set; }
        public Rectangle PaddingLayout { get; private set; }
        public Spacer ActualBorder { get; private set; }
        public BorderRadius ActualRadius { get; private set; }

        public override IEnumerable<IComponent> GetChildren()
        {
            if (Inner != null)
                yield return Inner;
        }

        protected override Size OnMeasure(Size size, IRendererContext context)
        {
            var panelSize = Margin.GetAbsoluteSize() + Padding.GetAbsoluteSize() + Border.AsSpacer().GetAbsoluteSize();

            Inner?.Measure((size - panelSize).Union(Size.Empty), context);

            return (Inner?.DesiredSize ?? Size.Empty) + panelSize;
        }

        protected override void OnArrange()
        {
            var (widthExpandable, heightExpandable) = WithExpandable(Margin, Border.AsSpacer(), Padding);
            var widths = widthExpandable.AsEnumerable().GetEnumerator();
            var heights = heightExpandable.AsEnumerable().GetEnumerator();

            BorderLayout = Layout.ShrinkBy(GetRealSize(Margin, widths, heights));
            PaddingLayout = BorderLayout.ShrinkBy(GetRealSize(Border.AsSpacer(), widths, heights));
            ActualBorder = CalculateActualBorder(PaddingLayout, BorderLayout);
            Inner?.Arrange(PaddingLayout.ShrinkBy(GetRealSize(Padding, widths, heights)));
            ActualRadius = CalculateActualRadius();
        }

        private BorderRadius CalculateActualRadius()
        {
            if (BorderRadius.Equals(BorderRadius.None))
                return BorderRadius;

            var maxRadius = Math.Min(BorderLayout.Width, BorderLayout.Height) * 0.5f;

            var tl = Math.Min(BorderRadius.TopLeft, maxRadius);
            var tr = Math.Min(BorderRadius.TopRight, maxRadius);
            var bl = Math.Min(BorderRadius.BottomLeft, maxRadius);
            var br = Math.Min(BorderRadius.BottomRight, maxRadius);

            return new BorderRadius(tl, tr, br, bl);
        }

        private static Spacer CalculateActualBorder(Rectangle padding, Rectangle border) => new Spacer(padding.Top - border.Top, padding.Left - border.Left, border.Bottom - padding.Bottom, border.Right - padding.Right);

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
