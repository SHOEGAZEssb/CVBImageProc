using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
  /// <summary>
  /// Median filter processor.
  /// </summary>
  [DataContract]
  [CustomFilterSettings]
  public class Median : FilterBase
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Median";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      var bounds = this.GetProcessingBounds(inputImage);
      if (ProcessAllPlanes)
      {
        foreach (var plane in inputImage.Planes)
          ProcessPlane(plane, bounds);
      }
      else
        ProcessPlane(inputImage.Planes[PlaneIndex], bounds);

      return inputImage;
    }

    private void ProcessPlane(ImagePlane plane, ProcessingBounds bounds)
    {
      var outputPlane = ProcessingHelper.ProcessMonoKernelParallel(plane, (kl) =>
      {
        var stripped = kl.Where(b => b.HasValue).ToArray();
        Array.Sort(stripped);
        return UseHigherMedian ? stripped[stripped.Length / 2].Value : stripped[(stripped.Length / 2) - 1].Value;
      }, KernelSize, bounds, PixelFilter);

      outputPlane.CopyTo(plane.Parent.Planes[plane.Plane]);
    }

    #endregion IProcessor Implementation

    #region Properties

    /// <summary>
    /// Indicates if the higher median value
    /// should be used instead of the lower one.
    /// </summary>
    [DataMember]
    public bool UseHigherMedian { get; set; } = true;

    #endregion Properties
  }
}