namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// RGB-Pixel struct.
  /// </summary>
  internal struct RGBPixel
  {
    #region Properties

    /// <summary>
    /// The R pixel.
    /// </summary>
    public byte R { get; }

    /// <summary>
    /// The G pixel.
    /// </summary>
    public byte G { get; }

    /// <summary>
    /// The B pixel.
    /// </summary>
    public byte B { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="r">The R pixel.</param>
    /// <param name="g">The G pixel.</param>
    /// <param name="b">The b pixel.</param>
    public RGBPixel(byte r, byte g, byte b)
    {
      R = r;
      G = g;
      B = b;
    }

    #endregion Construction
  }
}