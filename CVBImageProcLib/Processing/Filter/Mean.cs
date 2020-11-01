using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
  /// <summary>
  /// Mean filter processor.
  /// </summary>
  [DataContract]
  public class Mean : FilterBase
  {
    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Mean";

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
        var stripped = kl.Where(b => b.HasValue);
        return (byte)(stripped.Sum(b => b.Value) / stripped.Count());
      }, KernelSize, this.GetProcessingBounds(plane.Parent), PixelFilter);

      outputPlane.CopyTo(plane.Parent.Planes[plane.Plane]);
    }
  }
}
