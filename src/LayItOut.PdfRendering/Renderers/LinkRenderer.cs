using LayItOut.Components;
using LayItOut.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace LayItOut.PdfRendering.Renderers
{
    class LinkRenderer : IComponentRenderer<XGraphics, Link>
    {
        public void Render(XGraphics graphics, Link link)
        {
            var brush = new XSolidBrush(link.FontColor.ToXColor());
            var font = new XFont(link.Font);
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Near
            };
            foreach (var area in link.TextLayout.Areas)
            {
                var rect = area.Area;
                rect.Offset(link.Layout.Location);
                rect.Intersect(link.Layout);
                if (!(rect.Width > 0) || !(rect.Height > 0))
                    continue;

                var xRect = rect.ToXRect();
                var rr = graphics.Transformer.WorldToDefaultPage(xRect);
                graphics.PdfPage.AddWebLink(new PdfRectangle(rr), link.Href);
                graphics.DrawString(area.Block.Text, font, brush,rect.ToXRect(), format);
            }
        }
    }
}