using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  class Invert : IProcessor
  {
    #region IProcessor Implementation

    public string Name => "Invert";

    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      foreach (var plane in inputImage.Planes)
        InvertPlane(plane, inputImage.Size);

      return inputImage;
    }

    private void InvertPlane(ImagePlane plane, Size2D imageSize)
    {
      var data = plane.GetLinearAccess();

      unsafe
      {
        for (int y = 0; y < imageSize.Height; y++)
        {
          byte* pLine = (byte*)(data.BasePtr + (int)data.YInc * y);

          for (int x = 0; x < imageSize.Width; x++)
          {
            byte* pPixel = pLine + (int)data.XInc * x;

            *pPixel = (byte)(255 - *pPixel);
          }
        }
      }
    }

    #endregion IProcessor Implementation
  }
}