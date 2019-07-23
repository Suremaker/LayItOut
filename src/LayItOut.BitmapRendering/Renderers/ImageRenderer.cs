using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering.Renderers
{
    class ImageRenderer : IComponentRenderer<RendererContext, LayItOut.Components.Image>
    {
        public void Render(RendererContext ctx, LayItOut.Components.Image component)
        {
            if (component.Src == null)
                return;
            ctx.Graphics.DrawImage(component.Src, component.ImageLayout, component.ImageSourceRegion, GraphicsUnit.Pixel);
        }
    }
}
