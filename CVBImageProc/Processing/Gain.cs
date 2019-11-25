using Stemmer.Cvb;
using System;
using System.Linq;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Applies gain to an image.
  /// </summary>
  class Gain : IProcessor
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Gain";

    /// <summary>
    /// Applies the <see cref="Gains"/>
    /// to the given <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to apply gain to.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (Gains == null)
        Gains = inputImage.Planes.Select(p => 0.0).ToArray();

      for (int i = 0; i < inputImage.Planes.Count; i++)
      {
        var planeData = inputImage.Planes[i].GetLinearAccess();
        double gain = Gains[i];

        unsafe
        {
          for (int y = 0; y < inputImage.Height; y++)
          {
            byte* pLine = (byte*)(planeData.BasePtr + (int)planeData.YInc * y);

            for (int x = 0; x < inputImage.Width; x++)
            {
              byte* pPixel = pLine + (int)planeData.XInc * x;

              byte curPixel = *pPixel;
              byte value = (byte)(curPixel + gain);
              if (!WrapAround)
              {
                if (curPixel + gain > 255)
                  value = 255;
                else if (curPixel + gain < 0)
                  value = 0;
              }

              *pPixel = value;
            }
          }
        }
      }

      return inputImage;
    }

    #endregion IProcessor Implementation

    #region Properties

    /// <summary>
    /// If true, pixel values wrap
    /// around at < 0 and > 255.
    /// </summary>
    public bool WrapAround { get; set; }

    /// <summary>
    /// The gain per image plane.
    /// </summary>
    public double[] Gains
    {
      get => _gains;
      set
      {
        if (value == null || value.Length == 0)
          throw new ArgumentNullException(nameof(Gains));
        else
          _gains = value;
      }
    }
    private double[] _gains;

    #endregion Properties
  }
}