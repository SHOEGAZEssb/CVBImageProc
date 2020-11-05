using CVBImageProcLib.Processing.PixelFilter;
using CVBImageProcLib.Processing.ValueProvider;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// ViewModel that replaces certain pixel values.
  /// </summary>
  [DataContract]
  public class Replace : AOIPlaneProcessorBase, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Replace";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      var bounds = this.GetProcessingBounds(inputImage);
      if (ProcessAllPlanes)
      {
        foreach (var plane in inputImage.Planes)
          ProcessPlane(plane, bounds);
      }
      else
        ProcessPlane(inputImage.Planes[PlaneIndex], bounds);

      return inputImage;
    }

    private void ProcessPlane(ImagePlane plane, ProcessingBounds bounds)
    {
      ProcessingHelper.ProcessMono(plane, bounds, (b) =>
      {
        return ValueProvider.Provide();
      }, PixelFilter);
    }

    #endregion IProcessor Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain of the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion ICanprocessIndividualPixel Implementation

    #region Properties

    /// <summary>
    /// Value to use when replacing.
    /// </summary>
    [DataMember]
    public ByteValueProvider ValueProvider { get; set; } = new ByteValueProvider(0, 255);

    #endregion Properties
  }
}