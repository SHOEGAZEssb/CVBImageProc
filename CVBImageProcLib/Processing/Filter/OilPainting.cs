using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
  /// <summary>
  /// Oil painting filter processor.
  /// </summary>
  [DataContract]
  [CustomFilterSettings]
  public class OilPainting : FilterBase
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "OilPainting";

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
        var numIntensityLevels = NumIntensityLevels;
        // the number of pixels in each intensity level range
        var intensityCount = new int[numIntensityLevels];
        // the sums of all pixel values in their corresponding intensity level range:
        var intensitySums = new int[numIntensityLevels];

        foreach (byte pixel in stripped)
        {
          int intensityLevel = (int)(pixel * (numIntensityLevels - 1) / 255.0);
          intensitySums[intensityLevel] += pixel;
          intensityCount[intensityLevel]++;
        }

        // the intensity level with the most pixels in
        int maxIndex = Array.IndexOf(intensityCount, intensityCount.Max());

        return (byte)(intensitySums[maxIndex] / intensityCount[maxIndex]);
      }, KernelSize, bounds, PixelFilter);

      outputPlane.CopyTo(plane.Parent.Planes[plane.Plane]);
    }

    #endregion IProcessor Implementation

    #region Properties

    /// <summary>
    /// /// The number of intensity levels
    /// the result image will have.
    /// </summary>
    [DataMember]
    public int NumIntensityLevels { get; set; } = 20;

    #endregion Properties
  }
}