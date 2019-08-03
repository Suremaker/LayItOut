using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering.Renderers
{
    class ImageRenderer : ComponentRenderer<BitmapRendererContext, LayItOut.Components.Image>
    {
        protected override void OnRender(BitmapRendererContext ctx, LayItOut.Components.Image component)
        {
            if (component.Src == null)
                return;
            ctx.Graphics.DrawImage(component.Src, component.ImageLayout, component.ImageSourceRegion, GraphicsUnit.Pixel);
        }
    }
}
