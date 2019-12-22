using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Inverts an image.
  /// </summary>
  [DataContract]
  class Invert : IProcessor, ICanProcessIndividualPixel, IProcessIndividualPlanes, ICanProcessIndividualRegions
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
              *pPixel = (byte)(255 - pixelValue);
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

    #region IProcessIndividualPlanes Implementation

    /// <summary>
    /// Index of the plane to invert.
    /// </summary>
    public int PlaneIndex { get; set; }

    #endregion IProcessIndividualPlanes Implementation
  }
}