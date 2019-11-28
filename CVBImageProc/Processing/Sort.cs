using Stemmer.Cvb;
using System;
using System.Linq;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Mode to use while sorting.
  /// </summary>
  enum SortMode
  {
    /// <summary>
    /// Pixels will be sorted in ascending order.
    /// </summary>
    Ascending,

    /// <summary>
    /// Pixels will be sorted in descending order.
    /// </summary>
    Descending
  }

  /// <summary>
  /// Processor that sorts an image plane.
  /// </summary>
  class Sort : IProcessor, IProcessIndividualPlanes
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Sort";

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
      int byteCounter = 0;
      byte[] sortedBytes;
      unsafe
      {
        if (Mode == SortMode.Ascending)
          sortedBytes = inputImage.Planes[PlaneIndex].AllPixels.Select(p => *(byte*)p).OrderBy(i => i).ToArray();
        else
          sortedBytes = inputImage.Planes[PlaneIndex].AllPixels.Select(p => *(byte*)p).OrderByDescending(i => i).ToArray();

        for (int y = 0; y < inputImage.Height; y++)
        {
          byte* pLine = (byte*)(planeData.BasePtr + (int)planeData.YInc * y);

          for (int x = 0; x < inputImage.Width; x++)
          {
            byte* pPixel = pLine + (int)planeData.XInc * x;

            *pPixel = sortedBytes[byteCounter++];
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

    #region Properties

    /// <summary>
    /// Sorting mode.
    /// </summary>
    public SortMode Mode { get; set; }

    #endregion Properties
  }
}