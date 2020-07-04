using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Pixel filter that checks if a given
  /// pixel index is dividable by the
  /// configured value.
  /// </summary>
  [DataContract]
  [DisplayName("Modulo (Index)")]
  public class ModuloIndex : PixelIndexFilterBase
  {
    /// <summary>
    /// Name of the filter.
    /// </summary>
    public override string Name => "Modulo (Index)";

    /// <summary>
    /// Minimum index value to compare to.
    /// </summary>
    public override int MinCompareValue => 1;

    /// <summary>
    /// Checks if the given <paramref name="index"/> passes the filter.
    /// </summary>
    /// <param name="index">Index to check.</param>
    /// <returns>True if the <paramref name="index"/> passes the filter,
    /// otherwise false.</returns>
    public override bool Check(int index)
    {
      try
      {
        return Invert ? index % CompareValue != 0 : index % CompareValue == 0;
      }
      catch (DivideByZeroException)
      {
        return false;
      }
    }

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public ModuloIndex()
    {
      CompareValue = MinCompareValue;
    }

    #endregion Construction
  }
}