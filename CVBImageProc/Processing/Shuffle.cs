using Stemmer.Cvb;
using System;
using System.Linq;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Processor that shuffles an image plane.
  /// </summary>
  class Shuffle : IProcessor, IProcessIndividualPlanes
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Shuffle";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      var rnd = new Random(DateTime.Now.Ticks.GetHashCode());
      var planeData = inputImage.Planes[PlaneIndex].GetLinearAccess();
      int byteCounter = 0;

      unsafe
      {
        byte[] shuffledBytes = inputImage.Planes[PlaneIndex].AllPixels.Select(p => *(byte*)p).OrderBy(i => rnd.Next()).ToArray();
        for (int y = 0; y < inputImage.Height; y++)
        {
          byte* pLine = (byte*)(planeData.BasePtr + (int)planeData.YInc * y);

          for (int x = 0; x < inputImage.Width; x++)
          {
            byte* pPixel = pLine + (int)planeData.XInc * x;

            *pPixel = shuffledBytes[byteCounter++];
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
  }
}
