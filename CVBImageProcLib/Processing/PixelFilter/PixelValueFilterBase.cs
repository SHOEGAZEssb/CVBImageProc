using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.PixelFilter
{
  /// <summary>
  /// Base class for pixel value filters.
  /// </summary>
  [DataContract]
  public abstract class PixelValueFilterBase : IPixelValueFilter
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
    public byte CompareByte
    {
      get => _compareByte;
      set
      {
        if (value > MaxCompareByte)
          value = MaxCompareByte;
        else if (value < MinCompareByte)
          value = MinCompareByte;

        _compareByte = value;
      }
    }
    private byte _compareByte;

    /// <summary>
    /// Max value of the <see cref="CompareByte"/>.
    /// </summary>
    public virtual byte MaxCompareByte => 255;

    /// <summary>
    /// Min value of the <see cref="CompareByte"/>.
    /// </summary>
    public virtual byte MinCompareByte => 0;

    /// <summary>
    /// If true, inverts the logic of the
    /// <see cref="Check(byte)"/>.
    /// </summary>
    [DataMember]
    public bool Invert { get; set; }

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