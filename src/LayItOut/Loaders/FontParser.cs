using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace LayItOut.Loaders
{
    public class FontParser : IFontLoader
    {
        private readonly PrivateFontCollection _custom = new PrivateFontCollection();
        public void Dispose()
        {
            _custom.Dispose();
        }

        public Font Parse(string value)
        {
            var parts = value.Split(';');
            var style = FontStyle.Regular;
            if (parts.Length < 2 || !float.TryParse(parts[1], out var size)
                                 || (parts.Length == 3 && !Enum.TryParse(parts[2], true, out style))
                                 || parts.Length > 3)
            {
                throw new ArgumentException($"Unable to parse {nameof(Font)}: {value}", nameof(value));
            }

            try
            {
                return new Font(GetFamily(parts[0]), size, style, GraphicsUnit.World);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Unable to parse {nameof(Font)}: {value} - {e.Message}", nameof(value), e);
            }
        }

        private FontFamily GetFamily(string family)
        {
            return _custom.Families.FirstOrDefault(f => f.Name == family) ?? new FontFamily(family);
        }

        public void AddFont(string fontFile)
        {
            _custom.AddFontFile(fontFile);
        }
    }
}