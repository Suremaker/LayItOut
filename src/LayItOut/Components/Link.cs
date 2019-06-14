using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    public class Link : Label
    {
        public string Href { get; set; }

        protected override TextMetadata GetTextMetadata()
        {
            return new TextMetadata(Font, FontColor, LineHeight) { LinkHref = Href };
        }
    }
}