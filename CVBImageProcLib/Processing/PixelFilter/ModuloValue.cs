using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.PixelFilter
{
  /// <summary>
  /// Pixel filter that checks if a given
  /// pixel value is dividable by the
  /// configured value.
  /// </summary>
  [DataContract]
  [DisplayName("Modulo (Value)")]
  public sealed class ModuloValue : PixelValueFilterBase
  {
    #region IPixelFilter Implementation

    /// <summary>
    /// Name of the filter.
    /// </summary>
    public override string Name => "Modulo (Value)";

    /// <summary>
    /// Min value of the <see cref="PixelValueFilterBase.CompareByte"/>.
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
        return Invert ? pixel % CompareByte != 0 : pixel % CompareByte == 0;
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
    public ModuloValue()
    {
      CompareByte = MinCompareByte;
    }

    #endregion Construction
  }
}