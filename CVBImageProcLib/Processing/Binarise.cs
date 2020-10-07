using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Processor for binarising an image.
  /// </summary>
  [DataContract]
  public class Binarise : IAOIPlaneProcessor, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Binarise";

    /// <summary>
    /// Binarises the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to binarise.</param>
    /// <returns>Binarised image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      ProcessingHelper.ProcessMono(inputImage.Planes[PlaneIndex], this.GetProcessingBounds(inputImage), (b) =>
      {
        return (byte)(b >= Threshold ? 255 : 0);
      }, PixelFilter);

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
    /// Threshold to use in the binarising process.
    /// Values &gt;= threshold = 255, values &lt; threshold = 0.
    /// </summary>
    [DataMember]
    public int Threshold
    {
      get => _threshold;
      set
      {
        if (value > MAXTHRESHOLD)
          value = MAXTHRESHOLD;
        else if (value < MINTHRESHOLD)
          value = MINTHRESHOLD;

        _threshold = value;
      }
    }
    private int _threshold = 128;

    /// <summary>
    /// Max value of the <see cref="Threshold"/>.
    /// </summary>
    public const int MAXTHRESHOLD = 254;

    /// <summary>
    /// Min value of the <see cref="Threshold"/>.
    /// </summary>
    public const int MINTHRESHOLD = 1;

    /// <summary>
    /// Red conversion factor.
    /// </summary>
    public const double FACTORRED = 0.2125;

    /// <summary>
    /// Green conversion factor.
    /// </summary>
    public const double FACTORGREEN = 0.7154;

    /// <summary>
    /// Blue conversion factor.
    /// </summary>
    public const double FACTORBLUE = 0.0721;

    #endregion Properties
  }
}