using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Applies gain to an image.
  /// </summary>
  [DataContract]
  class Gain : IProcessor, ICanProcessIndividualPixel, IProcessIndividualPlanes
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Gain";

    /// <summary>
    /// Applies the <see cref="GainValue"/>
    /// to the given <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to apply gain to.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      var planeData = inputImage.Planes[PlaneIndex].GetLinearAccess();

      int startY = 0;
      int startX = 0;
      int height = inputImage.Height;
      int width = inputImage.Width;
      if (UseAOI)
      {
        startY = AOI.Location.Y;
        startX = AOI.Location.X;
        height = AOI.Size.Height;
        width = AOI.Size.Width;
      }

      unsafe
      {
        for (; startY < height; startY++)
        {
          byte* pLine = (byte*)(planeData.BasePtr + (int)planeData.YInc * startY);

          for (int x = startX; x < width; x++)
          {
            byte* pPixel = pLine + (int)planeData.XInc * x;

            byte pixelValue = *pPixel;
            if (PixelFilter.Check(pixelValue))
            {
              byte value = (byte)(pixelValue + GainValue);
              if (!WrapAround)
              {
                if (pixelValue + GainValue > 255)
                  value = 255;
                else if (pixelValue + GainValue < 0)
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

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    public PixelFilterChain PixelFilter { get; private set; } = new PixelFilterChain();

    [DataMember]
    public bool UseAOI { get; set; }

    [DataMember]
    public Rect AOI { get; set; }

    #endregion ICanProcessIndividualPixel Implementation

    #region IProcessIndividualPlanes Implementation

    /// <summary>
    /// Index of the plane to invert.
    /// </summary>
    public int PlaneIndex { get; set; }

    #endregion IProcessIndividualPlanes Implementation

    #region Properties

    /// <summary>
    /// If true, pixel values wrap
    /// around at &lt; 0 and &gt; 255.
    /// </summary>
    [DataMember]
    public bool WrapAround { get; set; }

    /// <summary>
    /// The gain value to apply.
    /// </summary>
    [DataMember]
    public double GainValue { get; set; }

    #endregion Properties
  }
}