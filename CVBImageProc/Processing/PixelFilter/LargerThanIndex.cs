using System.Runtime.Serialization;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Pixel filter that checks if a given
  /// pixel index is larger than the
  /// configured value.
  /// </summary>
  [DataContract]
  public class LargerThanIndex : PixelIndexFilterBase
  {
    /// <summary>
    /// Name of the filter.
    /// </summary>
    public override string Name => "Larger Than (Index)";

    /// <summary>
    /// Checks if the given <paramref name="index"/> passes the filter.
    /// </summary>
    /// <param name="index">Index to check.</param>
    /// <returns>True if the <paramref name="index"/> passes the filter,
    /// otherwise false.</returns>
    public override bool Check(int index)
    {
      return Invert ? index < CompareValue : index > CompareValue;
    }
  }
}