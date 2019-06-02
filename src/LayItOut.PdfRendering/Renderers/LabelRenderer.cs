using LayItOut.Components;
using LayItOut.Rendering;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering.Renderers
{
    //TODO: better tests
    class LabelRenderer : IComponentRenderer<XGraphics, Label>
    {
        public void Render(XGraphics graphics, Label label)
        {
            var brush = new XSolidBrush(label.FontColor.ToXColor());
            var font = new XFont(label.Font);
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Near
            };
            foreach (var area in label.TextLayout.Areas)
            {
                var rect = area.Area;
                rect.Offset(label.Layout.Location);
                rect.Intersect(label.Layout);
                if (!(rect.Width > 0) || !(rect.Height > 0))
                    continue;

                graphics.DrawString(area.Block.Text, font, brush, rect.ToXRect(), format);
            }
        }
    }
}
