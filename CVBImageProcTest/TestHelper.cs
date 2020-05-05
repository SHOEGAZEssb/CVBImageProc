using Stemmer.Cvb;
using System;

namespace CVBImageProcTest
{
  static class TestHelper
  {
    public static Image CreateMonoTestImage(byte[] pixelValues)
    {
      var root = Math.Sqrt(pixelValues.Length);
      int sizeX = (int)Math.Floor(root);
      int sizeY = (int)Math.Ceiling(root);
      var img = new Image(sizeX, sizeY, 1);

      var data = img.Planes[0].GetLinearAccess();

      int curPixel = 0;
      unsafe
      {
        for (int y = 0; y < img.Height; y++)
        {
          byte* pLine = (byte*)(data.BasePtr + (int)data.YInc * y);

          for (int x = 0; x < img.Width; x++)
          {
            *(pLine + (int)data.XInc * x) = pixelValues[curPixel++];
          }
        }
      }

      return img;
    }
  }
}