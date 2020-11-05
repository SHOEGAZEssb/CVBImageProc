using System;
using System.Linq;
using System.Net.Security;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
  /// <summary>
  /// Base class for filters that apply
  /// a weighted kernel.
  /// </summary>
  [DataContract]
  public abstract class WeightedFilterBase : FilterBase
  {
    /// <summary>
    /// Applies the given <paramref name="weights"/> to
    /// the given <paramref name="values"/>.
    /// </summary>
    /// <param name="values">Byte values to apply weights to.</param>
    /// <param name="weights">Weights to apply.</param>
    /// <returns>Weighted pixel.</returns>
    protected virtual byte ApplyWeights(byte?[] values, int[] weights, int weightSum)
    {
      if (values == null)
        throw new ArgumentNullException(nameof(values));
      if (weights == null)
        throw new ArgumentNullException(nameof(weights));

      var intVals = new int?[values.Length];
      for (int i = 0; i < values.Length; i++)
      {
        if (values[i].HasValue)
          intVals[i] = values[i].Value * weights[i];
      }

      var stripped = intVals.Where(b => b.HasValue);
      int sum = weightSum == 0 ? stripped.Sum(v => v.Value) : stripped.Sum(v => v.Value) / weightSum;
      if (sum > 255)
        return 255;
      else if (sum < 0)
        return 0;
      return (byte)sum;
    }

    /// <summary>
    /// Converts the given binomial factors
    /// to weights.
    /// </summary>
    /// <param name="binFactors">Binomial factors to convert.</param>
    /// <returns>Weights calculated with the binomial factors.</returns>
    protected virtual int[] MakeWeights(int[] binFactors)
    {
      if (binFactors == null)
        throw new ArgumentNullException(nameof(binFactors));

      int[] weights = new int[binFactors.Length * binFactors.Length];
      int counter = 0;
      for (int i = 0; i < binFactors.Length; i++)
      {
        for (int e = 0; e < binFactors.Length; e++)
        {
          weights[counter++] = binFactors[i] * binFactors[e];
        }
      }

      return weights;
    }
  }
}