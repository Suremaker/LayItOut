using LayItOut.Attributes;
using LayItOut.Caching;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering
{
    public class PdfFontResolver : CachingResolver<FontInfo, XFont>
    {
        protected override XFont Create(FontInfo i) => new XFont(i.Family, i.Size, i.Style.ToXFontStyle());
    }
}