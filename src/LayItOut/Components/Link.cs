using System.ComponentModel;
using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    [Description("A link component allowing to include text in the layout with **Uri** metadata.")]
    public class Link : Label
    {
        [Description("Link reference.")]
        public string Uri { get; set; }

        protected override TextMetadata GetTextMetadata()
        {
            return new TextMetadata(Font, TextColor, LineHeight) { LinkHref = Uri };
        }
    }
}