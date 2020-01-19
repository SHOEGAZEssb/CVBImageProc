using Stemmer.Cvb;
using System;
using System.Collections.Generic;

namespace CVBImageProcTest
{
  /// <summary>
  /// Extensions for a <see cref="Image"/>.
  /// </summary>
  static class ImageExtensions
  {
    /// <summary>
    /// Gets all pixel values in the image.
    /// </summary>
    /// <param name="img">Image to get pixel values of.</param>
    /// <returns>Pixel values.</returns>
    public static IEnumerable<byte> GetPixels(this Image img)
    {
      if (img == null)
        throw new ArgumentNullException(nameof(img));

      var data = img.Planes[0].GetLinearAccess();

      var pixels = new byte[img.Width * img.Height];
      int curPixel = 0;
      unsafe
      {
        for (int y = 0; y < img.Height; y++)
        {
          byte* pLine = (byte*)(data.BasePtr + (int)data.YInc * y);

          for (int x = 0; x < img.Width; x++)
          {
            pixels[curPixel++] = *(pLine + (int)data.XInc * x);
          }
        }
      }

      return pixels;
    }
  }
}