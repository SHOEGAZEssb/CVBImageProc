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
  public class Binarise : FullProcessorBase
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
      ProcessingHelper.ProcessMono(plane, bounds, (b) =>
      {
        return (byte)(b >= Threshold ? 255 : 0);
      }, PixelFilter);
    }

    #endregion IProcessor Implementation

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