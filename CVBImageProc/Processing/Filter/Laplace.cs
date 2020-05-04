using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.Filter
{
  [DataContract]
  class Laplace : FilterBase
  {
    public override string Name => "Laplace";

    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      int kernelSize = (int)Math.Floor(KernelSize.GetKernelNumber() / 2.0);
      var plane = ProcessingHelper.ProcessMonoKernel(inputImage.Planes[PlaneIndex], (kl) =>
      {
        return ApplyWeights(kl, ThreeByThreeWeights);
      }, KernelSize, this.GetProcessingBounds(inputImage), PixelFilter);

      plane.CopyTo(inputImage.Planes[PlaneIndex]);
      return inputImage;
    }

    private static int[] ThreeByThreeWeights;

    static Laplace()
    {
      ThreeByThreeWeights = new int[]{ 0, 1, 0,
                                       1, -4, 1,
                                       0, 1, 0 };
    }

    private byte ApplyWeights(byte?[] values, int[] weights)
    {
      var intVals = new int?[values.Length];
      for (int i = 0; i < values.Length; i++)
      {
        if (values[i].HasValue)
          intVals[i] = values[i].Value * weights[i];
      }

      var stripped = intVals.Where(b => b.HasValue);
      int sum = stripped.Sum(v => v.Value);
      if (sum > 255)
        return 255;
      else if(sum < 0)
        return 0;
      return (byte)sum;
    }
  }
}
