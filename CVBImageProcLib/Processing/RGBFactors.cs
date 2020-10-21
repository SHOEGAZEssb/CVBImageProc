using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Processor for converting an rgb image using factors..
  /// </summary>
  [DataContract]
  [DisplayName("RGB Factors")]
  public class RGBFactors : IProcessor, ICanProcessIndividualPixel, ICanProcessIndividualRegions
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "RGB Factors";

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
        byte r = WrapAroundR ? (byte)ProcessingHelper.ClampPixelValue((rgb.R * FactorRR) + (rgb.G * FactorRG) + (rgb.B * FactorRB))
                             : (byte)((rgb.R * FactorRR) + (rgb.G * FactorRG) + (rgb.B * FactorRB));
        byte g = WrapAroundG ? (byte)ProcessingHelper.ClampPixelValue((rgb.R * FactorGR) + (rgb.G * FactorGG) + (rgb.B * FactorGB))
                             : (byte)((rgb.R * FactorGR) + (rgb.G * FactorGG) + (rgb.B * FactorGB));
        byte b = WrapAroundB ? (byte)ProcessingHelper.ClampPixelValue((rgb.R * FactorBR) + (rgb.G * FactorBG) + (rgb.B * FactorBB))
                             : (byte)((rgb.R * FactorBR) + (rgb.G * FactorBG) + (rgb.B * FactorBB));

        return new RGBPixel(r, g, b);
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

    #region Properties

    /// <summary>
    /// Factor of the R component of the R pixel.
    /// </summary>
    [DataMember]
    public double FactorRR { get; set; }

    /// <summary>
    /// Factor of the G component of the R pixel.
    /// </summary>
    [DataMember]
    public double FactorRG { get; set; }

    /// <summary>
    /// Factor of the B component of the R pixel.
    /// </summary>
    [DataMember]
    public double FactorRB { get; set; }

    /// <summary>
    /// Factor of the R component of the G pixel.
    /// </summary>
    [DataMember]
    public double FactorGR { get; set; }

    /// <summary>
    /// Factor of the G component of the G pixel.
    /// </summary>
    [DataMember]
    public double FactorGG { get; set; }

    /// <summary>
    /// Factor of the B component of the G pixel.
    /// </summary>
    [DataMember]
    public double FactorGB { get; set; }

    /// <summary>
    /// Factor of the R component of the B pixel.
    /// </summary>
    [DataMember]
    public double FactorBR { get; set; }

    /// <summary>
    /// Factor of the G component of the B pixel.
    /// </summary>
    [DataMember]
    public double FactorBG { get; set; }

    /// <summary>
    /// Factor of the B component of the B pixel.
    /// </summary>
    [DataMember]
    public double FactorBB { get; set; }

    /// <summary>
    /// If true, clamps the pixel values to 255 and 0,
    /// to stop them from overflowing.
    /// </summary>
    [DataMember]
    public bool WrapAroundR { get; set; } = true;

    /// <summary>
    /// If true, clamps the pixel values to 255 and 0,
    /// to stop them from overflowing.
    /// </summary>
    [DataMember]
    public bool WrapAroundG { get; set; } = true;

    /// <summary>
    /// If true, clamps the pixel values to 255 and 0,
    /// to stop them from overflowing.
    /// </summary>
    [DataMember]
    public bool WrapAroundB { get; set; } = true;

    #endregion Properties
  }
}