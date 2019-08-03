using LayItOut.Components;
using LayItOut.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace LayItOut.PdfRendering.Renderers
{
    class TextRenderer<T> : ComponentRenderer<PdfRendererContext, T> where T : ITextComponent, IComponent
    {
        protected override void OnRender(PdfRendererContext ctx, T element)
        {
            var graphics = ctx.Graphics;
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Near
            };

            foreach (var area in element.TextLayout.Areas)
            {
                var rect = area.Area;
                rect.Offset(element.Layout.Location);

                if (!(rect.Width > 0) || !(rect.Height > 0))
                    continue;

                var meta = area.Block.Metadata;

                if (meta.LinkHref != null)
                {
                    graphics.PdfPage.AddWebLink(
                        new PdfRectangle(graphics.Transformer.WorldToDefaultPage(rect.ToXRect())),
                        meta.LinkHref);
                }

                var state = graphics.Save();

                graphics.IntersectClip(element.Layout.ToXRect());
                graphics.DrawString(
                    area.Block.Text,
                    ctx.FontResolver.Resolve(meta.Font),
                    new XSolidBrush(meta.Color.ToXColor()),
                    rect.ToXRect(),
                    format);

                graphics.Restore(state);
            }
        }
    }
}