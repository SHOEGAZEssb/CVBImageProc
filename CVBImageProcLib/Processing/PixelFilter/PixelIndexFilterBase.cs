using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.PixelFilter
{
  /// <summary>
  /// Base class for all pixel index filters.
  /// </summary>
  [DataContract]
  public abstract class PixelIndexFilterBase : IPixelIndexFilter
  {
    #region IPixelIndexFilter Implementation

    /// <summary>
    /// Index value to compare to.
    /// </summary>
    [DataMember]
    public int CompareValue
    {
      get => _compareValue;
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException(nameof(CompareValue), $"{nameof(CompareValue)} can't be smaller than 0");

        _compareValue = value;
      }
    }
    private int _compareValue;

    /// <summary>
    /// Name of the filter.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// If true, inverts the logic of the filter.
    /// </summary>
    [DataMember]
    public bool Invert { get; set; }

    /// <summary>
    /// Minimum index value to compare to.
    /// </summary>
    public virtual int MinCompareValue => 0;

    /// <summary>
    /// Checks if the given <paramref name="index"/> passes the filter.
    /// </summary>
    /// <param name="index">Index to check.</param>
    /// <returns>True if the <paramref name="index"/> passes the filter,
    /// otherwise false.</returns>
    public abstract bool Check(int index);

    #endregion IPixelIndexFilter Implementation
  }
}