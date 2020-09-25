using CVBImageProc.Processing.PixelFilter;
using CVBImageProc.Processing.ValueProvider;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel that replaces certain pixel values.
  /// </summary>
  [DataContract]
  public class Replace : IAOIPlaneProcessor, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Replace";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      ProcessingHelper.ProcessMono(inputImage.Planes[PlaneIndex], this.GetProcessingBounds(inputImage), (b) =>
      {
        return ValueProvider.Provide();
      }, PixelFilter);

      return inputImage;
    }

    #endregion IProcessor Implementation

    #region IProcessIndividualPlanes Implementation

    /// <summary>
    /// Index of the plane to invert.
    /// </summary>
    [DataMember]
    public int PlaneIndex { get; set; }

    #endregion IProcessIndividualPlanes Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain of the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion ICanprocessIndividualPixel Implementation

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

    #region Properties

    /// <summary>
    /// Value to use when replacing.
    /// </summary>
    [DataMember]
    public ByteValueProvider ValueProvider { get; set; } = new ByteValueProvider(0, 255);

    #endregion Properties
  }
}