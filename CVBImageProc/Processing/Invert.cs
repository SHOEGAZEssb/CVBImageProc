using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Inverts an image.
  /// </summary>
  [DataContract]
  class Invert : IProcessor
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Invert";

    /// <summary>
    /// Inverts the given <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage"></param>
    /// <returns></returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      foreach (var plane in inputImage.Planes)
        InvertPlane(plane, inputImage.Size);

      return inputImage;
    }

    /// <summary>
    /// Inverts the pixel values of the
    /// given <paramref name="plane"/>.
    /// </summary>
    /// <param name="plane">Plane to invert.</param>
    /// <param name="imageSize">Size of the plane.</param>
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