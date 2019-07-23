using System.ComponentModel;
using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    [Description("A link component allowing to include text in the layout with **Href** metadata.")]
    public class Link : Label
    {
        [Description("Link reference.")]
        public string Href { get; set; }

        protected override TextMetadata GetTextMetadata()
        {
            return new TextMetadata(Font, FontColor, LineHeight) { LinkHref = Href };
        }
    }
}