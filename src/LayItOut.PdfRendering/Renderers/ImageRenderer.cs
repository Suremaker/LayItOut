using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Rendering;
using PdfSharp.Drawing;
using Image = LayItOut.Components.Image;

namespace LayItOut.PdfRendering.Renderers
{
    class ImageRenderer : ComponentRenderer<PdfRendererContext, Image>
    {
        protected override void OnRender(PdfRendererContext ctx, Image component)
        {
            if (component.Src.IsNone)
                return;
            var img = ctx.GetBitmap(component.Src);
            if (component.Scaling != ImageScaling.None)
                ctx.Graphics.DrawImage(img, component.ImageLayout.ToXRect());
            else
                DrawClipped(ctx, component, img);
        }

        private static void DrawClipped(PdfRendererContext ctx, Image component, XImage img)
        {
            var state = ctx.Graphics.Save();
            try
            {
                ctx.Graphics.IntersectClip(component.ImageLayout.ToXRect());
                var location = component.ImageLayout.Location;
                location.X -= component.ImageSourceRegion.Location.X;
                location.Y -= component.ImageSourceRegion.Location.Y;
                var rect = new RectangleF(location, component.MeasuredImageSize);
                ctx.Graphics.DrawImage(img, rect.ToXRect());
            }
            finally
            {
                ctx.Graphics.Restore(state);
            }
        }
    }
}
