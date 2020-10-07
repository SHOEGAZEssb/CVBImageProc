using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.PixelFilter
{
  /// <summary>
  /// Pixel filter that checks if a given
  /// pixel value is equal the configured value.
  /// </summary>
  [DataContract]
  [DisplayName("Equals (Value)")]
  public class EqualsValue : PixelValueFilterBase
  {
    #region IPixelFilter Implementation

    /// <summary>
    /// Name of the filter.
    /// </summary>
    public override string Name => "Equals (Value)";

    /// <summary>
    /// Checks if the given <paramref name="pixel"/>
    /// passes the filter.
    /// </summary>
    /// <param name="pixel">Pixel to check.</param>
    /// <returns>True if the <paramref name="pixel"/> passes
    /// the filter, otherwise false.</returns>
    public override bool Check(byte pixel)
    {
      return Invert ? pixel != CompareByte : pixel == CompareByte;
    }

    #endregion IPixelFilter Implementation
  }
}
