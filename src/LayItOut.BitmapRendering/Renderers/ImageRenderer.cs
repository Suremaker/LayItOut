using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering.Renderers
{
    class ImageRenderer : IComponentRenderer<Graphics, LayItOut.Components.Image>
    {
        public void Render(Graphics graphics, LayItOut.Components.Image component)
        {
            if (component.Src == null)
                return;
            graphics.DrawImage(component.Src, component.Layout, new Rectangle(Point.Empty, component.Src.Size), GraphicsUnit.Pixel);
        }
    }
}
