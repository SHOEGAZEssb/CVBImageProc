namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Pixel filter that checks if a given
  /// pixel value is larger than the
  /// configured value.
  /// </summary>
  class LargerThan : PixelFilterBase
  {
    #region IPixelFilter Implementation

    /// <summary>
    /// Name of the filter.
    /// </summary>
    public override string Name => "Larger Than";

    /// <summary>
    /// Checks if the given <paramref name="pixel"/>
    /// passes the filter.
    /// </summary>
    /// <param name="pixel">Pixel to check.</param>
    /// <returns>True if the <paramref name="pixel"/> passes
    /// the filter, otherwise false.</returns>
    public override bool Check(byte pixel)
    {
      return Not ? CompareByte > pixel : CompareByte < pixel;
    }

    #endregion IPixelFilter Implementation
  }
}