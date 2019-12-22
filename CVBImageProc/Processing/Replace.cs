using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel that replaces certain pixel values.
  /// </summary>
  [DataContract]
  class Replace : IProcessor, IProcessIndividualPlanes, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Replace";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
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

            if (PixelFilter.Check(*pPixel))
              *pPixel = ReplaceWith;
          }
        }
      }

      return inputImage;
    }

    #endregion IProcessor Implementation

    #region IProcessIndividualPlanes Implementation

    /// <summary>
    /// Index of the plane to invert.
    /// </summary>
    public int PlaneIndex { get; set; }

    #endregion IProcessIndividualPlanes Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain of the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; } = new PixelFilterChain();

    [DataMember]
    public bool UseAOI { get; set; }

    [DataMember]
    public Rect AOI { get; set; }

    #endregion ICanprocessIndividualPixel Implementation

    #region Properties

    /// <summary>
    /// Value to use when replacing.
    /// </summary>
    public byte ReplaceWith { get; set; }

    #endregion Properties
  }
}