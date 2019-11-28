using System;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Pixel filter that checks if a given
  /// pixel value is dividable by the
  /// configured value.
  /// </summary>
  [DataContract]
  class Modulo : PixelFilterBase
  {
    #region IPixelFilter Implementation

    /// <summary>
    /// Name of the filter.
    /// </summary>
    public override string Name => "Modulo";

    /// <summary>
    /// Min value of the <see cref="PixelFilterBase.CompareByte"/>.
    /// </summary>
    public override byte MinCompareByte => 1;

    /// <summary>
    /// Checks if the given <paramref name="pixel"/>
    /// passes the filter.
    /// </summary>
    /// <param name="pixel">Pixel to check.</param>
    /// <returns>True if the <paramref name="pixel"/> passes
    /// the filter, otherwise false.</returns>
    public override bool Check(byte pixel)
    {
      try
      {
        return Not ? pixel % CompareByte != 0 : pixel % CompareByte == 0;
      }
      catch (DivideByZeroException)
      {
        return false;
      }
    }

    #endregion IPixelFilter Implementation

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public Modulo()
    {
      CompareByte = MinCompareByte;
    }

    #endregion Construction
  }
}