using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    public class Image : Component
    {
        public Bitmap Src { get; set; }

        protected override Size OnMeasure(Size size, IRendererContext context)
        {
            return Src?.Size ?? Size.Empty;
        }
    }
}
