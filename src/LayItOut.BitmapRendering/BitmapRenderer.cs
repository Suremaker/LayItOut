using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using LayItOut.BitmapRendering.Renderers;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering
{
    public class BitmapRenderer : Renderer<Graphics>
    {
        private readonly Action<Graphics> _configureGraphics;

        public BitmapRenderer(Action<Graphics> configureGraphics = null)
        {
            RegisterRenderer(new PanelRenderer());
            _configureGraphics = configureGraphics;
        }

        public void Render(Form form, Bitmap target)
        {
            form.LayOut(target.Size);
            using (var g = Graphics.FromImage(target))
            {
                ConfigureGraphics(g);
                Render(g, form.Content);
            }
        }

        private void ConfigureGraphics(Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _configureGraphics?.Invoke(g);
        }
    }
}
