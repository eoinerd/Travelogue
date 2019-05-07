using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Travelogue.Extensions
{
    public class ImageResizeHelper
    {
        public static void ResizeImageToSquare(string path)
        {
            // Use ImageMagic (3rd party) to resize to perfect square for profile pic
            using (Image<Rgba32> imageMagic = Image.Load(path))
            {
                var wideImage = imageMagic.Width - imageMagic.Height;
                var highImage = imageMagic.Height - imageMagic.Width;

                if (highImage > 0)
                {
                    imageMagic.Mutate(x => x.Crop(imageMagic.Width, imageMagic.Width));
                }
                else if (wideImage > 0)
                {
                    imageMagic.Mutate(x => x.Crop(imageMagic.Height, imageMagic.Height));
                }

                imageMagic.Save(path); // Automatic encoder selected based on extension.
            }
        }
    }
}
