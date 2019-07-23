using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using LayItOut.BitmapRendering;
using LayItOut.Loaders;

namespace LayItOut.DocGen
{
    public class SnippetCompiler
    {
        private readonly FormLoader _loader;
        private readonly BitmapRenderer _renderer;
        public static readonly SnippetCompiler Instance = new SnippetCompiler();
        private SnippetCompiler()
        {
            _loader = new FormLoader();
            _renderer = new BitmapRenderer();
        }

        public void Compile(string name, string snippet)
        {
            var form = _loader.LoadForm(new StringReader(snippet));
            var opt = new BitmapRendererOptions
            {
                ConfigureGraphics = g => { g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; }
            };
            using (var bmp = _renderer.Render(form, opt))
            {
                bmp.Save($"man\\images\\{name}");
            }
        }
    }
}
