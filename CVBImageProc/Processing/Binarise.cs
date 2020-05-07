using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Processor for binarising an image.
  /// </summary>
  [DataContract]
  public class Binarise : IProcessor, ICanProcessIndividualPixel, ICanProcessIndividualRegions
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Binarise";

    /// <summary>
    /// Binarises the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to binarise.</param>
    /// <returns>Binarised image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (inputImage.Planes.Count == 1)
        return BinariseMono(inputImage);
      else if (inputImage.Planes.Count >= 3)
        return BinariseRGB(inputImage);
      else
        throw new ArgumentException("Given input image not supported for binarising", nameof(inputImage));
    }

    /// <summary>
    /// Binarises the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to binarise.</param>
    /// <returns>Binarised image.</returns>
    private Image BinariseMono(Image inputImage)
    {
      ProcessingHelper.ProcessMono(inputImage.Planes[0], this.GetProcessingBounds(inputImage), (b) =>
      {
        return (byte)(b >= Threshold ? 255 : 0);
      }, PixelFilter);

      return inputImage;
    }

    /// <summary>
    /// Binarises the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to binarise.</param>
    /// <returns>Binarised image.</returns>
    private Image BinariseRGB(Image inputImage)
    {
      var rData = inputImage.Planes[0].GetLinearAccess();
      var gData = inputImage.Planes[1].GetLinearAccess();
      var bData = inputImage.Planes[2].GetLinearAccess();

      var newImage = new Image(inputImage.Size, 1, inputImage.Planes[0].DataType);
      var mData = newImage.Planes[0].GetLinearAccess();

      var bounds = this.GetProcessingBounds(inputImage);
      int boundHeight = inputImage.Height - 1;
      int boundWidth = inputImage.Width - 1;
      int boundsY = bounds.StartY + bounds.Height;
      int boundsX = bounds.StartX + bounds.Width;

      int rYInc = (int)rData.YInc;
      int gYInc = (int)gData.YInc;
      int bYInc = (int)bData.YInc;
      int rXInc = (int)rData.XInc;
      int gXInc = (int)gData.XInc;
      int bXInc = (int)bData.XInc;
      int mXInc = (int)mData.XInc;
      int mYInc = (int)mData.YInc;

      unsafe
      {
        for (int y = bounds.StartY; y < boundsY; y++)
        {
          byte* pLineR = (byte*)(rData.BasePtr + rYInc * y);
          byte* pLineG = (byte*)(gData.BasePtr + gYInc * y);
          byte* pLineB = (byte*)(bData.BasePtr + bYInc * y);
          byte* pLineNew = (byte*)(mData.BasePtr + mYInc * y);

          for (int x = bounds.StartX; x < boundsX; x++)
          {
            byte* pPixelR = pLineR + rXInc * x;
            byte* pPixelG = pLineR + gXInc * x;
            byte* pPixelB = pLineR + bXInc * x;
            byte* pPixelNew = pLineNew + mXInc * x;

            byte value = 0;
            byte pixelValue = (byte)(*pPixelR * FACTORRED + *pPixelG * FACTORGREEN + *pPixelB * FACTORBLUE);
            if (PixelFilter.Check(pixelValue, y * inputImage.Height + x))
            {
              if (pixelValue >= Threshold)
                value = 255;

              *pPixelNew = value;
            }
          }
        }
      }

      return newImage;
    }

    #endregion IProcessor Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion ICanProcessIndividualPixel Implementation

    #region ICanProcessIndividualRegions Implementation

    /// <summary>
    /// If true, uses the <see cref="AOI"/>
    /// while processing.
    /// </summary>
    [DataMember]
    public bool UseAOI { get; set; }

    /// <summary>
    /// The AOI to process.
    /// </summary>
    [DataMember]
    public Rect AOI { get; set; }

    #endregion ICanProcessIndividualRegions Implementation

    #region Properties

    /// <summary>
    /// Threshold to use in the binarising process.
    /// Values &gt;= threshold = 255, values &lt; threshold = 0.
    /// </summary>
    [DataMember]
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

    /// <summary>
    /// Red conversion factor.
    /// </summary>
    public const double FACTORRED = 0.2125;

    /// <summary>
    /// Green conversion factor.
    /// </summary>
    public const double FACTORGREEN = 0.7154;

    /// <summary>
    /// Blue conversion factor.
    /// </summary>
    public const double FACTORBLUE = 0.0721;

    #endregion Properties
  }
}