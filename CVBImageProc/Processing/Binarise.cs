using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Processor for binarising an image.
  /// </summary>
  class Binarise : IProcessor
  {
    #region Properties

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Binarise";

    /// <summary>
    /// Threshold to use in the binarising process.
    /// Values >= threshold = 255, values < threshold = 0.
    /// </summary>
    public int Threshold
    {
      get => _threshold;
      set
      {
        if (value > MAXTHRESHOLD)
          value = MAXTHRESHOLD;
        else if (value < MINTHRESHOLD)
          value = MINTHRESHOLD;

        _threshold = value;
      }
    }
    private int _threshold = 128;

    /// <summary>
    /// Max value of the <see cref="Threshold"/>.
    /// </summary>
    public const int MAXTHRESHOLD = 254;

    /// <summary>
    /// Min value of the <see cref="Threshold"/>.
    /// </summary>
    public const int MINTHRESHOLD = 1;

    public const double FACTORRED = 0.2125;
    public const double FACTORGREEN = 0.7154;
    public const double FACTORBLUE = 0.0721;

    #endregion Properties

    /// <summary>
    /// Binarises the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (inputImage.Planes.Count == 1)
        return BinariseMono(inputImage);
      else if (inputImage.Planes.Count == 3)
        return BinariseRGB(inputImage);
      else
        throw new ArgumentException("Given input image not supported for binarising", nameof(inputImage));
    }

    private Image BinariseMono(Image inputImage)
    {
      var data = inputImage.Planes[0].GetLinearAccess();

      unsafe
      {
        for (int y = 0; y < inputImage.Height; y++)
        {
          byte* pLine = (byte*)(data.BasePtr + (int)data.YInc * y);

          for (int x = 0; x < inputImage.Width; x++)
          {
            byte* pPixel = pLine + (int)data.XInc * x;
            *pPixel = (byte)(*pPixel >= Threshold ? 255 : 0);
          }
        }
      }

      return inputImage;
    }

    private Image BinariseRGB(Image inputImage)
    {
      var dataR = inputImage.Planes[0].GetLinearAccess();
      var dataG = inputImage.Planes[1].GetLinearAccess();
      var dataB = inputImage.Planes[2].GetLinearAccess();

      unsafe
      {
        for (int y = 0; y < inputImage.Height; y++)
        {
          byte* pLineR = (byte*)(dataR.BasePtr + (int)dataR.YInc * y);
          byte* pLineG = (byte*)(dataG.BasePtr + (int)dataG.YInc * y);
          byte* pLineB = (byte*)(dataB.BasePtr + (int)dataB.YInc * y);

          for (int x = 0; x < inputImage.Width; x++)
          {
            byte* pPixelR = pLineR + (int)dataR.XInc * x;
            byte* pPixelG = pLineR + (int)dataG.XInc * x;
            byte* pPixelB = pLineR + (int)dataB.XInc * x;

            byte value = 0;
            if (*pPixelR * FACTORRED + *pPixelG * FACTORGREEN + *pPixelB * FACTORBLUE >= Threshold)
              value = 255;

            *pPixelR = value;
            *pPixelG = value;
            *pPixelB = value;
          }
        }
      }

      return inputImage;
    }
  }
}