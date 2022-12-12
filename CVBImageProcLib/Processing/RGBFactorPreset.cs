namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Preset for the <see cref="RGBFactors"/> processor.
  /// </summary>
  public readonly struct RGBFactorPreset
  {
    #region Properties

    /// <summary>
    /// Factor of the R component of the R pixel.
    /// </summary>
    public double FactorRR { get; }

    /// <summary>
    /// Factor of the G component of the R pixel.
    /// </summary>
    public double FactorRG { get; }

    /// <summary>
    /// Factor of the B component of the R pixel.
    /// </summary>
    public double FactorRB { get; }

    /// <summary>
    /// Factor of the R component of the G pixel.
    /// </summary>
    public double FactorGR { get; }

    /// <summary>
    /// Factor of the G component of the G pixel.
    /// </summary>
    public double FactorGG { get; }

    /// <summary>
    /// Factor of the B component of the G pixel.
    /// </summary>
    public double FactorGB { get; }

    /// <summary>
    /// Factor of the R component of the B pixel.
    /// </summary>
    public double FactorBR { get; }

    /// <summary>
    /// Factor of the G component of the B pixel.
    /// </summary>
    public double FactorBG { get; }

    /// <summary>
    /// Factor of the B component of the B pixel.
    /// </summary>
    public double FactorBB { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="factorRR">Factor of the R component of the R pixel.</param>
    /// <param name="factorRG">Factor of the G component of the R pixel.</param>
    /// <param name="factorRB">Factor of the B component of the R pixel.</param>
    /// <param name="factorGR">Factor of the R component of the G pixel.</param>
    /// <param name="factorGG">Factor of the G component of the G pixel.</param>
    /// <param name="factorGB">Factor of the B component of the G pixel.</param>
    /// <param name="factorBR">Factor of the R component of the B pixel.</param>
    /// <param name="factorBG">Factor of the G component of the B pixel.</param>
    /// <param name="factorBB">Factor of the B component of the B pixel.</param>
    public RGBFactorPreset(double factorRR, double factorRG, double factorRB,
                           double factorGR, double factorGG, double factorGB,
                           double factorBR, double factorBG, double factorBB)
    {
      FactorRR = factorRR;
      FactorRG = factorRG;
      FactorRB = factorRB;
      FactorGR = factorGR;
      FactorGG = factorGG;
      FactorGB = factorGB;
      FactorBR = factorBR;
      FactorBG = factorBG;
      FactorBB = factorBB;
    }

    #endregion Construction
  }
}