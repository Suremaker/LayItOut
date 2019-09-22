using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Threading.Tasks;
using LayItOut.Attributes;
using LayItOut.BitmapRendering;
using LayItOut.Components;
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
            _loader = new FormLoader(new AssetLoader(null, name => File.ReadAllBytesAsync($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{name}")));
            _renderer = new BitmapRenderer();
        }

        public async Task Compile(string name, string snippet)
        {
            var form = await _loader.LoadForm(new StringReader(snippet));
            WrapForm(form);
            var opt = new BitmapRendererOptions
            {
                ConfigureGraphics = g => { g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; }
            };
            using (var bmp = _renderer.Render(form, opt))
            {
                bmp.Save($"man\\assets\\{name}");
            }
        }

        private void WrapForm(Form form)
        {
            form.UpdateContent(new Panel
            {
                Border = new Border(1, Color.Black),
                Padding = new Spacer(1),
                Inner = form.Content
            });
        }
    }
}
