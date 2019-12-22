using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Processor that shuffles an image plane.
  /// </summary>
  [DataContract]
  class Shuffle : IProcessor, IProcessIndividualPlanes, ICanProcessIndividualRegions
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

      byte[] shuffledBytes;
      unsafe
      {
        if(UseAOI)
          shuffledBytes = inputImage.Planes[PlaneIndex].GetAllPixelsIn(AOI).Select(p => *(byte*)p).OrderBy(i => rnd.Next()).ToArray();
        else
          shuffledBytes = inputImage.Planes[PlaneIndex].AllPixels.Select(p => *(byte*)p).OrderBy(i => rnd.Next()).ToArray();

        for (; startY < height; startY++)
        {
          byte* pLine = (byte*)(planeData.BasePtr + (int)planeData.YInc * startY);

          for (int x = startX; x < width; x++)
          {
            byte* pPixel = pLine + (int)planeData.XInc * x;

            *pPixel = shuffledBytes[byteCounter++];
          }
        }
      }

      return inputImage;
    }

    #endregion IProcessor Implementation

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
