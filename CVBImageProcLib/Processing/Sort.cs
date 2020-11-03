using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Mode to use while sorting.
  /// </summary>
  public enum SortMode
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
  [DataContract]
  public class Sort : AOIPlaneProcessorBase, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Sort";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (ProcessAllPlanes)
      {
        foreach (var plane in inputImage.Planes)
          ProcessPlane(plane);
      }
      else
        ProcessPlane(inputImage.Planes[PlaneIndex]);

      return inputImage;
    }

    private void ProcessPlane(ImagePlane plane)
    {
      int byteCounter = 0;
      byte[] sortedBytes;

      unsafe
      {
        if (UseAOI)
        {
          if (Mode == SortMode.Ascending)
            sortedBytes = plane.GetAllPixelsIn(AOI).Select(p => *(byte*)p).OrderBy(i => i).ToArray();
          else
            sortedBytes = plane.GetAllPixelsIn(AOI).Select(p => *(byte*)p).OrderByDescending(i => i).ToArray();
        }
        else
        {
          if (Mode == SortMode.Ascending)
            sortedBytes = plane.AllPixels.Select(p => *(byte*)p).OrderBy(i => i).ToArray();
          else
            sortedBytes = plane.AllPixels.Select(p => *(byte*)p).OrderByDescending(i => i).ToArray();
        }
      }

      ProcessingHelper.ProcessMono(plane, this.GetProcessingBounds(plane.Parent), (b) =>
      {
        return sortedBytes[byteCounter++];
      }, PixelFilter);
    }

    #endregion IProcessor Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion ICanProcessIndividualPixel Implementation

    #region Properties

    /// <summary>
    /// Sorting mode.
    /// </summary>
    [DataMember]
    public SortMode Mode { get; set; }

    #endregion Properties
  }
}