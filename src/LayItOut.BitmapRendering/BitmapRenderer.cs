using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using LayItOut.BitmapRendering.Renderers;
using LayItOut.Components;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering
{
    public class BitmapRenderer : Renderer<BitmapRendererContext>
    {
        private readonly bool _disposeFontResolver;
        public BitmapFontResolver FontResolver { get; }

        public BitmapRenderer(BitmapFontResolver fontResolver = null)
        {
            _disposeFontResolver = fontResolver == null;
            FontResolver = fontResolver ?? new BitmapFontResolver();
            RegisterRenderer(new PanelRenderer());
            RegisterRenderer(new TextRenderer<Link>());
            RegisterRenderer(new TextRenderer<Label>());
            RegisterRenderer(new TextRenderer<TextBox>());
            RegisterRenderer(new ImageRenderer());
            RegisterRenderer(new ViewportRenderer());
        }

        public void Render(Form form, Bitmap target, BitmapRendererOptions options = null)
        {
            options = options ?? BitmapRendererOptions.Default;

            using (var localBitmapCache = new BitmapCache())
            using (var g = CreateGraphics(target, options))
            {
                var context = CreateContext(g, localBitmapCache);
                form.LayOut(target.Size, context);
                Render(context, form.Content);
            }
        }

        public Bitmap Render(Form form, BitmapRendererOptions options = null)
        {
            options = options ?? BitmapRendererOptions.Default;

            using (var localBitmapCache = new BitmapCache())
            {
                using (var refBmp = new Bitmap(1, 1))
                using (var refGraphics = CreateGraphics(refBmp, options))
                    form.LayOut(new Size(int.MaxValue, int.MaxValue), CreateContext(refGraphics, localBitmapCache));

                var bitmap = new Bitmap(form.Content.DesiredSize.Width, form.Content.DesiredSize.Height,
                    PixelFormat.Format32bppArgb);
                using (var graphics = CreateGraphics(bitmap, options))
                    Render(CreateContext(graphics, localBitmapCache), form.Content);

                return bitmap;
            }
        }

        private BitmapRendererContext CreateContext(Graphics graphics, BitmapCache localBitmapCache)
        {
            return new BitmapRendererContext(graphics, FontResolver, localBitmapCache);
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

        public override void Dispose()
        {
            if (_disposeFontResolver)
                FontResolver.Dispose();
            base.Dispose();
        }
    }
}
