using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.Filter
{
  /// <summary>
  /// Min filter processor.
  /// </summary>
  [DataContract]
  public class Max : FilterBase
  {
    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Max";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      int kernelSize = (int)System.Math.Floor(base.KernelSize.GetKernelNumber() / 2.0);
      var plane = ProcessingHelper.ProcessMonoKernel(inputImage.Planes[PlaneIndex], (kl) =>
      {
        return kl.Where(b => b.HasValue).Max(b => b.Value);
      }, KernelSize, this.GetProcessingBounds(inputImage), PixelFilter);

      plane.CopyTo(inputImage.Planes[PlaneIndex]);
      return inputImage;
    }
  }
}