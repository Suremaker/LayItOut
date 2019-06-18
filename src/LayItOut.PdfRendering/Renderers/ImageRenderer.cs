using System.Drawing.Imaging;
using System.IO;
using LayItOut.Components;
using LayItOut.Rendering;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering.Renderers
{
    class ImageRenderer : IComponentRenderer<XGraphics, Image>
    {
        public void Render(XGraphics graphics, Image component)
        {
            if (component.Src == null)
                return;

            using (var mem = new MemoryStream())
            {
                component.Src.Save(mem, ImageFormat.Png);
                mem.Seek(0, SeekOrigin.Begin);
                using (var img = XImage.FromStream(mem))
                    graphics.DrawImage(img, component.Layout.ToXRect());
            }
        }
    }
}
