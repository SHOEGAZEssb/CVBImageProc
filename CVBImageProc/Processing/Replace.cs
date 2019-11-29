using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel that replaces certain pixel values.
  /// </summary>
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
      unsafe
      {
        for (int y = 0; y < inputImage.Height; y++)
        {
          byte* pLine = (byte*)(planeData.BasePtr + (int)planeData.YInc * y);

          for (int x = 0; x < inputImage.Width; x++)
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
    public PixelFilterChain PixelFilter { get; } = new PixelFilterChain();

    #endregion ICanprocessIndividualPixel Implementation

    #region Properties

    /// <summary>
    /// Value to use when replacing.
    /// </summary>
    public byte ReplaceWith { get; set; }

    #endregion Properties
  }
}