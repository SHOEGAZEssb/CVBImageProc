using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.Filter
{
  /// <summary>
  /// Filter processor with custom weights.
  /// </summary>
  [DataContract]
  [CustomFilterSettings]
  public class Custom : WeightedFilterBase
  {
    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Custom";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      int kernelSize = (int)System.Math.Floor(KernelSize.GetKernelNumber() / 2.0);
      var plane = ProcessingHelper.ProcessMonoKernel(inputImage.Planes[PlaneIndex], (kl) =>
      {
        return ApplyWeights(kl, Weights);
      }, KernelSize, this.GetProcessingBounds(inputImage), PixelFilter);

      plane.CopyTo(inputImage.Planes[PlaneIndex]);
      return inputImage;
    }

    #region Properties

    /// <summary>
    /// Kernel size to use.
    /// </summary>
    [IgnoreDataMember]
    public override KernelSize KernelSize
    {
      get => _kernelSize;
      set
      {
        if (KernelSize != value)
        {
          _kernelSize = value;
          CreateWeights();
        }
      }
    }
    [DataMember]
    private KernelSize _kernelSize;

    /// <summary>
    /// The custom weights.
    /// </summary>
    [DataMember]
    public int[] Weights { get; set; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public Custom()
    {
      CreateWeights();
    }

    #endregion Construction

    /// <summary>
    /// Creates the <see cref="Weights"/>
    /// based on the <see cref="KernelSize"/>.
    /// </summary>
    private void CreateWeights()
    {
      int num = KernelSize.GetKernelNumber();
      Weights = new int[num * num];
    }
  }
}