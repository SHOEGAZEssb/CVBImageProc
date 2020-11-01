using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
  /// <summary>
  /// Min filter processor.
  /// </summary>
  [DataContract]
  public class Min : FilterBase
  {
    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Min";

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
      var outputPlane = ProcessingHelper.ProcessMonoKernel(plane, (kl) =>
      {
        return kl.Where(b => b.HasValue).Min(b => b.Value);
      }, KernelSize, this.GetProcessingBounds(plane.Parent), PixelFilter);

      outputPlane.CopyTo(plane.Parent.Planes[plane.Plane]);
    }
  }
}