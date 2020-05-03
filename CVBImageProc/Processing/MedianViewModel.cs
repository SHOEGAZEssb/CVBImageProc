using CVBImageProc.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Median"/> processor.
  /// </summary>
  class MedianViewModel : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Kernel size to use.
    /// </summary>
    public KernelSize KernelSize
    {
      get => _processor.KernelSize;
      set
      {
        if(KernelSize != value)
        {
          _processor.KernelSize = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public PixelFilterChainViewModel PixelFilterChainVM { get; }

    /// <summary>
    /// ViewModel for the AOI.
    /// </summary>
    public AOIViewModel AOIVM { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly Median _processor;

    #endregion Member

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    public MedianViewModel(Median processor) 
      : base(processor)
    {
      _processor = processor;
      PixelFilterChainVM = new PixelFilterChainViewModel(_processor);
      PixelFilterChainVM.SettingsChanged += SubVM_SettingsChanged;
      AOIVM = new AOIViewModel(_processor);
      AOIVM.SettingsChanged += SubVM_SettingsChanged;
    }

    /// <summary>
    /// Fires the SettingsChanged event when the
    /// pixel filter settings changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void SubVM_SettingsChanged(object sender, EventArgs e)
    {
      OnSettingsChanged();
    }
  }
}