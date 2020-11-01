using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
  /// <summary>
  /// Main processor for filter processors.
  /// </summary>
  [DataContract]
  [DisplayName("Filter")]
  public class FilterProcessor : AOIPlaneProcessorBase, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => SelectedFilter == null ? "Filter (None)" : $"Filter ({SelectedFilter.Name})";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (SelectedFilter == null)
        return inputImage;

      ConfigureFilter();
      return SelectedFilter.Process(inputImage);
    }

    #endregion IProcessorImplementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion ICanProcessIndividualPixel Implementation

    #region Properties

    /// <summary>
    /// The sub filter to use.
    /// </summary>
    [DataMember]
    public IFilter SelectedFilter { get; set; }

    /// <summary>
    /// Kernel size to use.
    /// </summary>
    [DataMember]
    public KernelSize KernelSize
    {
      get => SelectedFilter?.KernelSize ?? KernelSize.ThreeByThree;
      set
      {
        if (SelectedFilter != null)
          SelectedFilter.KernelSize = value;
      }
    }

    #endregion Properties

    /// <summary>
    /// Configures the <see cref="SelectedFilter"/>
    /// with the configured values of this filter.
    /// </summary>
    private void ConfigureFilter()
    {
      if (SelectedFilter is ICanProcessIndividualPixel p)
        p.PixelFilter = PixelFilter;
      if (SelectedFilter is IProcessIndividualPlanes i)
      {
        i.PlaneIndex = PlaneIndex;
        i.ProcessAllPlanes = ProcessAllPlanes;
      }
      if (SelectedFilter is ICanProcessIndividualRegions r)
      {
        r.AOI = AOI;
        r.UseAOI = UseAOI;
      }
    }
  }
}