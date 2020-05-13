using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Processor for converting an rgb image to sepia.
  /// </summary>
  [DataContract]
  [DisplayName("RGB To Sepia")]
  public class RGBToSepia : IProcessor, ICanProcessIndividualPixel, ICanProcessIndividualRegions
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "RGB To Sepia";

    /// <summary>
    /// Applies the gain value
    /// to the given <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to apply gain to.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      ProcessingHelper.ProcessRGB(inputImage, (rgb) =>
      {
        byte r = (byte)ProcessingHelper.RestrictPixelValue((rgb.Item1 * FactorRR) + (rgb.Item2 * FactorRG) + (rgb.Item3 * FactorRB));
        byte g = (byte)ProcessingHelper.RestrictPixelValue((rgb.Item1 * FactorGR) + (rgb.Item2 * FactorGG) + (rgb.Item3 * FactorGB));
        byte b = (byte)ProcessingHelper.RestrictPixelValue((rgb.Item1 * FactorBR) + (rgb.Item2 * FactorBG) + (rgb.Item3 * FactorBB));

        return new Tuple<byte, byte, byte>(r, g, b);
      }, this.GetProcessingBounds(inputImage), PixelFilter);

      return inputImage;
    }

    #endregion IProcessor Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion ICanProcessIndividualPixel Implementation

    #region ICanProcessIndividualRegions Implementation

    /// <summary>
    /// If true, uses the <see cref="AOI"/>
    /// while processing.
    /// </summary>
    [DataMember]
    public bool UseAOI { get; set; }

    /// <summary>
    /// The AOI to process.
    /// </summary>
    [DataMember]
    public Rect AOI { get; set; }

    #endregion ICanProcessIndividualRegions Implementation

    #region IProcessIndividualPlanes Implementation

    /// <summary>
    /// Index of the plane to invert.
    /// </summary>
    [DataMember]
    public int PlaneIndex { get; set; }

    #endregion IProcessIndividualPlanes Implementation

    #region Consts

    /// <summary>
    /// Default RR factor.
    /// </summary>
    public const double DEFAULTFACTORRR = 0.393;

    /// <summary>
    /// Default RG factor.
    /// </summary>
    public const double DEFAULTFACTORRG = 0.769;

    /// <summary>
    /// Default RB factor.
    /// </summary>
    public const double DEFAULTFACTORRB = 0.189;

    /// <summary>
    /// Default GR factor.
    /// </summary>
    public const double DEFAULTFACTORGR = 0.349;

    /// <summary>
    /// Default GG factor.
    /// </summary>
    public const double DEFAULTFACTORGG = 0.686;

    /// <summary>
    /// Default GB factor.
    /// </summary>
    public const double DEFAULTFACTORGB = 0.168;

    /// <summary>
    /// Default BR factor.
    /// </summary>
    public const double DEFAULTFACTORBR = 0.272;

    /// <summary>
    /// Default BG factor.
    /// </summary>
    public const double DEFAULTFACTORBG = 0.534;

    /// <summary>
    /// Default BB factor.
    /// </summary>
    public const double DEFAULTFACTORBB = 0.131;

    #endregion Consts

    #region Properties

    /// <summary>
    /// Factor of the R component of the R pixel.
    /// </summary>
    [DataMember]
    public double FactorRR { get; set; } = DEFAULTFACTORRR;

    /// <summary>
    /// Factor of the G component of the R pixel.
    /// </summary>
    [DataMember]
    public double FactorRG { get; set; } = DEFAULTFACTORRG;

    /// <summary>
    /// Factor of the B component of the R pixel.
    /// </summary>
    [DataMember]
    public double FactorRB { get; set; } = DEFAULTFACTORRB;

    /// <summary>
    /// Factor of the R component of the G pixel.
    /// </summary>
    [DataMember]
    public double FactorGR { get; set; } = DEFAULTFACTORGR;

    /// <summary>
    /// Factor of the G component of the G pixel.
    /// </summary>
    [DataMember]
    public double FactorGG { get; set; } = DEFAULTFACTORGG;

    /// <summary>
    /// Factor of the B component of the G pixel.
    /// </summary>
    [DataMember]
    public double FactorGB { get; set; } = DEFAULTFACTORGB;

    /// <summary>
    /// Factor of the R component of the B pixel.
    /// </summary>
    [DataMember]
    public double FactorBR { get; set; } = DEFAULTFACTORBR;

    /// <summary>
    /// Factor of the G component of the B pixel.
    /// </summary>
    [DataMember]
    public double FactorBG { get; set; } = DEFAULTFACTORBG;

    /// <summary>
    /// Factor of the B component of the B pixel.
    /// </summary>
    [DataMember]
    public double FactorBB { get; set; } = DEFAULTFACTORBB;

    #endregion Properties
  }
}