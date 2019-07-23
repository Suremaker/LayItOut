using System.Drawing;
using LayItOut.Components;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering.Renderers
{
    class TextRenderer<T> : IComponentRenderer<RendererContext, T> where T : ITextComponent, IComponent
    {
        public void Render(RendererContext ctx, T element)
        {
            var graphics = ctx.Graphics;
            foreach (var area in element.TextLayout.Areas)
            {
                var rect = area.Area;
                rect.Offset(element.Layout.Location);

                if (!(rect.Width > 0) || !(rect.Height > 0))
                    continue;

                var meta = area.Block.Metadata;

                var state = graphics.Save();

                graphics.IntersectClip(element.Layout);
                using (var brush = new SolidBrush(meta.Color))
                    graphics.DrawString(
                        area.Block.Text,
                        ctx.FontResolver.Resolve(meta.Font),
                        brush,
                        rect, StringFormat.GenericTypographic);

                graphics.Restore(state);
            }
        }
    }
}