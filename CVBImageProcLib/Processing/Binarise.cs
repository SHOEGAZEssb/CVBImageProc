using CVBImageProcLib.Processing.PixelFilter;
using CVBImageProcLib.Processing.ValueProvider;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Processor for binarising an image.
  /// </summary>
  [DataContract]
  public sealed class Binarise : FullProcessorBase
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Binarise";

    /// <summary>
    /// Binarises the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to binarise.</param>
    /// <returns>Binarised image.</returns>
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
      ProcessingHelper.ProcessMonoParallel(plane, bounds, (b) =>
      {
        return b >= Threshold.Provide() ? AboveThresholdValue.Provide() : BelowThresholdValue.Provide();
      }, PixelFilter);
    }

    #endregion IProcessor Implementation

    #region Properties

    /// <summary>
    /// Threshold to use in the binarising process.
    /// Values &gt;= threshold = 255, values &lt; threshold = 0.
    /// </summary>
    [DataMember]
    public IntValueProvider Threshold = new IntValueProvider(0, 255, 128);

    /// <summary>
    /// Value to use if the byte value is larger or equal than the <see cref="Threshold"/>.
    /// </summary>
    [DataMember]
    public ByteValueProvider AboveThresholdValue = new ByteValueProvider(0, 255, 255);

    /// <summary>
    /// Value to use if the byte value is smaller than the <see cref="Threshold"/>.
    /// </summary>
    [DataMember]
    public ByteValueProvider BelowThresholdValue = new ByteValueProvider(0, 255);

    #endregion Properties
  }
}