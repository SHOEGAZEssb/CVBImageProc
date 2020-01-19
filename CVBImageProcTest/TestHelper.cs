using Stemmer.Cvb;

namespace CVBImageProcTest
{
  static class TestHelper
  {
    public static Image CreateMonoTestImage(byte[] pixelValues)
    {
      int size = pixelValues.Length / 2;
      var img = new Image(size, size, 1);

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