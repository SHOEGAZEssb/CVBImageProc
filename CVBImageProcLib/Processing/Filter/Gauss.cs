using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
  /// <summary>
  /// Gauss filter processor.
  /// </summary>
  [DataContract]
  public class Gauss : WeightedFilterBase
  {
    /// <summary>
    /// Name of the filter.
    /// </summary>
    public override string Name => "Gauss";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      int[] factors = MakeBinominalFactors(KernelSize.GetKernelNumber());
      int[] weights = MakeWeights(factors);

      var bounds = this.GetProcessingBounds(inputImage);
      int weightSum = weights.Sum();
      if (ProcessAllPlanes)
      {
        foreach (var plane in inputImage.Planes)
          ProcessPlane(plane, weights, bounds, weightSum);
      }
      else
        ProcessPlane(inputImage.Planes[PlaneIndex], weights, bounds, weightSum);

      return inputImage;
    }

    private void ProcessPlane(ImagePlane plane, int[] weights, ProcessingBounds bounds, int weightSum)
    {
      var outputPlane = ProcessingHelper.ProcessMonoKernelParallel(plane, (kl) =>
      {
        return ApplyWeights(kl, weights, weightSum);
      }, KernelSize, bounds, PixelFilter);

      outputPlane.CopyTo(plane.Parent.Planes[plane.Plane]);
    }

    /// <summary>
    /// Creates the binominal factors for the given <paramref name="kernelSize"/>.
    /// </summary>
    /// <param name="kernelSize">Size of the 1d kernel.</param>
    /// <returns>Binominal factors for the given <paramref name="kernelSize"/>.</returns>
    private static int[] MakeBinominalFactors(int kernelSize)
    {
      int[] factors = new int[kernelSize];
      for (int i = 0; i < factors.Length; i++)
        factors[i] = MakeBinominalFactor(factors.Length - 1, i);

      return factors; ;
    }

    private static int MakeBinominalFactor(int n, int k)
    {
      int res;
      if (2 * k > n)
        k = n - k;
      res = 1;
      for (int i = 1; i <= k; i++)
        res = res * (n - k + i) / i;
      return res;
    }
  }
}