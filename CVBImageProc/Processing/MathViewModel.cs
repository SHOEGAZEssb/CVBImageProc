using System;
using CVBImageProc.Processing.PixelFilter;
using CVBImageProc.Processing.ValueProvider;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Math"/> processor.
  /// </summary>
  class MathViewModel : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Math mode to use while processing.
    /// </summary>
    public MathMode Mode
    {
      get => _processor.Mode;
      set
      {
        if (Mode != value)
        {
          _processor.Mode = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// If true, pixel values wrap
    /// around at &lt; 0 and &gt; 255.
    /// </summary>
    public bool WrapAround
    {
      get => _processor.WrapAround;
      set
      {
        if (WrapAround != value)
        {
          _processor.WrapAround = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// ViewModel for providing gain values.
    /// </summary>
    public ValueProviderViewModel<int> ValueProviderVM { get; }

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
    /// The gain processor.
    /// </summary>
    private readonly Math _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The gain processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public MathViewModel(Math processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
      ValueProviderVM = new ValueProviderViewModel<int>(_processor.ValueProvider);
      ValueProviderVM.SettingsChanged += SubVM_SettingsChanged;
      PixelFilterChainVM = new PixelFilterChainViewModel(_processor);
      PixelFilterChainVM.SettingsChanged += SubVM_SettingsChanged;
      AOIVM = new AOIViewModel(_processor);
      AOIVM.SettingsChanged += SubVM_SettingsChanged;
    }

    #endregion Construction

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