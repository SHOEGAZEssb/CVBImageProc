using System.Runtime.Serialization;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Base class for pixel filters.
  /// </summary>
  [DataContract]
  abstract class PixelFilterBase : IPixelFilter
  {
    #region IPixelFilter Implementation

    /// <summary>
    /// Name of the filter.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Byte to compare to.
    /// </summary>
    [DataMember]
    public byte CompareByte { get; set; }

    /// <summary>
    /// If true, inverts the logic of the
    /// <see cref="Check(byte)"/>.
    /// </summary>
    [DataMember]
    public bool Not { get; set; }

    /// <summary>
    /// Checks if the given <paramref name="pixel"/>
    /// passes the filter.
    /// </summary>
    /// <param name="pixel">Pixel to check.</param>
    /// <returns>True if the <paramref name="pixel"/> passes
    /// the filter, otherwise false.</returns>
    public abstract bool Check(byte pixel);

    #endregion IPixelFilter Implementation
  }
}