using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.Filter
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

      int kernelSize = (int)Math.Floor(KernelSize.GetKernelNumber() / 2.0);
      var plane = ProcessingHelper.ProcessMonoKernel(inputImage.Planes[PlaneIndex], (kl) =>
      {
        var stripped = kl.Where(b => b.HasValue).ToArray();
        Array.Sort(stripped);
        return UseHigherMedian ? stripped[stripped.Length / 2].Value : stripped[(stripped.Length / 2) - 1].Value;
      }, KernelSize, this.GetProcessingBounds(inputImage), PixelFilter);

      plane.CopyTo(inputImage.Planes[PlaneIndex]);
      return inputImage;
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