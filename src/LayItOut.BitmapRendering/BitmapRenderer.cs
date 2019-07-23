﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using LayItOut.BitmapRendering.Renderers;
using LayItOut.Components;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering
{
    public class BitmapRenderer : Renderer<RendererContext>
    {
        public FontResolver FontResolver { get; }

        public BitmapRenderer(FontResolver fontResolver = null)
        {
            FontResolver = fontResolver ?? new FontResolver();
            RegisterRenderer(new PanelRenderer());
            RegisterRenderer(new TextRenderer<Link>());
            RegisterRenderer(new TextRenderer<Label>());
            RegisterRenderer(new TextRenderer<TextBox>());
            RegisterRenderer(new ImageRenderer());
        }

        public void Render(Form form, Bitmap target, BitmapRendererOptions options = null)
        {
            options = options ?? BitmapRendererOptions.Default;

            using (var g = CreateGraphics(target, options))
            {
                var context = new RendererContext(g, FontResolver);
                form.LayOut(target.Size, context);
                Render(context, form.Content);
            }
        }

        public Bitmap Render(Form form, BitmapRendererOptions options = null)
        {
            options = options ?? BitmapRendererOptions.Default;

            using (var refBmp = new Bitmap(1, 1))
            using (var refGraphics = CreateGraphics(refBmp, options))
                form.LayOut(new Size(int.MaxValue, int.MaxValue), new RendererContext(refGraphics, FontResolver));

            var bitmap = new Bitmap(form.Content.DesiredSize.Width, form.Content.DesiredSize.Height, PixelFormat.Format32bppArgb);
            using (var graphics = CreateGraphics(bitmap, options))
                Render(new RendererContext(graphics, FontResolver), form.Content);

            return bitmap;
        }

        private Graphics CreateGraphics(Bitmap target, BitmapRendererOptions options)
        {
            var g = Graphics.FromImage(target);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            options.ConfigureGraphics?.Invoke(g);
            return g;
        }
    }
}
