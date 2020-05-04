using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.Filter
{
  /// <summary>
  /// Main processor for filter processors.
  /// </summary>
  [DataContract]
  [DisplayName("Filter")]
  public class FilterProcessor : IProcessor, ICanProcessIndividualPixel, IProcessIndividualPlanes, ICanProcessIndividualRegions
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => SelectedFilter == null ? "Filter (None)" : $"Filter ({SelectedFilter.Name})";     

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
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
    /// The sub filter to use.
    /// </summary>
    [DataMember]
    public IFilter SelectedFilter { get; set; }

    /// <summary>
    /// Kernel size to use.
    /// </summary>
    [DataMember]
    public KernelSize KernelSize { get; set; }

    #endregion Properties

    /// <summary>
    /// Configures the <see cref="SelectedFilter"/>
    /// with the configured values of this filter.
    /// </summary>
    private void ConfigureFilter()
    {
      SelectedFilter.KernelSize = KernelSize;

      if (SelectedFilter is FilterBase f)
      {
        f.PixelFilter = PixelFilter;
        f.PlaneIndex = PlaneIndex;
        f.AOI = AOI;
        f.UseAOI = UseAOI;
      }
      else
      {
        if (SelectedFilter is ICanProcessIndividualPixel p)
          p.PixelFilter = PixelFilter;
        if (SelectedFilter is IProcessIndividualPlanes i)
          i.PlaneIndex = PlaneIndex;
        if (SelectedFilter is ICanProcessIndividualRegions r)
        {
          r.AOI = AOI;
          r.UseAOI = UseAOI;
        }
      }
    }
  }
}