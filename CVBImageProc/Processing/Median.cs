using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Kernel size for filter processor.
  /// </summary>
  public enum KernelSize
  {
    /// <summary>
    /// 3x3 kernel.
    /// </summary>
    [Description("3x3")]
    ThreeByThree,

    /// <summary>
    /// 5x5 kernel.
    /// </summary>
    [Description("5x5")]
    FiveByFive,

    /// <summary>
    /// 7x7 kernel.
    /// </summary>
    [Description("7x7")]
    SevenBySeven
  }

  /// <summary>
  /// Median filter processor.
  /// </summary>
  [DataContract]
  public class Median : IProcessor, ICanProcessIndividualPixel, IProcessIndividualPlanes, ICanProcessIndividualRegions
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Median";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      int kernelSize = (int)Math.Floor(KernelSize.GetKernelNumber() / 2.0);
      var plane = ProcessingHelper.ProcessMonoKernel(inputImage.Planes[PlaneIndex], (kl) =>
      {
        kl.Sort();
        return kl[kl.Count / 2];
      }, KernelSize, this.GetProcessingBounds(inputImage), PixelFilter);

      plane.CopyTo(inputImage.Planes[PlaneIndex]);
      return inputImage;
    }

    #endregion IProcessorImplementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; private set; } = new PixelFilterChain();

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
    /// Kernel size to use.
    /// </summary>
    [DataMember]
    public KernelSize KernelSize { get; set; }

    #endregion Properties
  }
}