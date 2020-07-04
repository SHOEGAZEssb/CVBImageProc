using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
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
  public class Sort : IProcessor, IProcessIndividualPlanes, ICanProcessIndividualRegions
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

      int byteCounter = 0;
      byte[] sortedBytes;

      unsafe
      {
        if (UseAOI)
        {
          if (Mode == SortMode.Ascending)
            sortedBytes = inputImage.Planes[PlaneIndex].GetAllPixelsIn(AOI).Select(p => *(byte*)p).OrderBy(i => i).ToArray();
          else
            sortedBytes = inputImage.Planes[PlaneIndex].GetAllPixelsIn(AOI).Select(p => *(byte*)p).OrderByDescending(i => i).ToArray();
        }
        else
        {
          if (Mode == SortMode.Ascending)
            sortedBytes = inputImage.Planes[PlaneIndex].AllPixels.Select(p => *(byte*)p).OrderBy(i => i).ToArray();
          else
            sortedBytes = inputImage.Planes[PlaneIndex].AllPixels.Select(p => *(byte*)p).OrderByDescending(i => i).ToArray();
        }
      }

      ProcessingHelper.ProcessMono(inputImage.Planes[PlaneIndex], this.GetProcessingBounds(inputImage), (b) =>
      {
        return sortedBytes[byteCounter++];
      });

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
    [DataMember]
    public int PlaneIndex { get; set; }

    #endregion IProcessIndividualPlanes Implementation

    #region Properties

    /// <summary>
    /// Sorting mode.
    /// </summary>
    [DataMember]
    public SortMode Mode { get; set; }

    #endregion Properties
  }
}