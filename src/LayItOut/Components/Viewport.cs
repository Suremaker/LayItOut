using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using LayItOut.Attributes;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    public class Viewport : Component, IWrappingComponent
    {
        [Description("Allows to clip the inner component from left,right,top and bottom directions")]
        public Spacer ClipMargin { get; set; }
        [Description("Specifies how inner component should be aligned within the view")]
        public Alignment ContentAlignment { get; set; }
        public IComponent Inner { get; set; }
        public Rectangle ActualViewRegion { get; private set; }

        protected override Size OnMeasure(Size size, IRendererContext context)
        {
            if (Inner == null)
                return Size.Empty;

            var marginSize = ClipMargin.GetAbsoluteSize();
            Inner.Measure(size.Add(marginSize), context);
            return Inner.DesiredSize.Subtract(marginSize);
        }

        protected override void OnArrange()
        {
            if (Inner == null)
                return;

            var clipShift = new Point(-ClipMargin.Left.AbsoluteOrDefault(), -ClipMargin.Top.AbsoluteOrDefault());
            var clipSize = Inner.DesiredSize.Subtract(ClipMargin.GetAbsoluteSize());
            var shift = CalculateAlignmentShift(clipSize);
            var location = Point.Empty;
            location.Offset(clipShift);
            location.Offset(shift);
            location.Offset(Layout.Location);
            Inner.Arrange(new Rectangle(location, Inner.DesiredSize));

            var viewRegionSize = Inner.DesiredSize.Subtract(new Size(
                ClipMargin.Right.AbsoluteOrDefault(),
                ClipMargin.Bottom.AbsoluteOrDefault()))
                .Subtract(new Size(Math.Max(0, shift.X), Math.Max(0, shift.Y)));


            var clipped = new Rectangle(location, viewRegionSize);
            clipped.Intersect(Layout);
            ActualViewRegion = clipped;

        }

        private Point CalculateAlignmentShift(Size clipSize)
        {
            var hshift = ContentAlignment.Horizontal.GetShift(Math.Abs(Layout.Size.Width - clipSize.Width)).Width *
                         (Layout.Size.Width > clipSize.Width ? 1 : -1);
            var vshift = ContentAlignment.Vertical.GetShift(Math.Abs(Layout.Size.Height - clipSize.Height)).Height *
                         (Layout.Size.Height > clipSize.Height ? 1 : -1);
            return new Point(hshift, vshift);
        }

        public override IEnumerable<IComponent> GetChildren()
        {
            if (Inner != null)
                yield return Inner;
        }
    }
}
