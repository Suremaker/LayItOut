using System.ComponentModel;
using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    [Description("An image component allowing to include images in the layout.\n\nWhen loaded via `FormLoader`, the images are resolved via `BitmapLoader` instance.")]
    public class Image : Component
    {
        [Description("Image source. When parsed from XML, it can be a string containing image name or inline image base64 encoded in with prefix `base64:`.")]
        public Bitmap Src { get; set; }

        protected override Size OnMeasure(Size size, IRendererContext context)
        {
            return Src?.Size ?? Size.Empty;
        }
    }
}
