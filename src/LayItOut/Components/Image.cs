using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    public class Image : Component
    {
        public Bitmap Src { get; set; }

        protected override Size OnMeasure(Size size, IRenderingContext context)
        {
            return Src?.Size ?? Size.Empty;
        }
    }
}
