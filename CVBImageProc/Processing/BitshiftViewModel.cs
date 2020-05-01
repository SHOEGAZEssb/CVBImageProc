using CVBImageProc.Processing.PixelFilter;
using CVBImageProc.Processing.ValueProvider;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="BitShift"/> processor.
  /// </summary>
  class BitshiftViewModel : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Direction to shift bits.
    /// </summary>
    public BitShiftDirection ShiftDirection
    {
      get => _processor.ShiftDirection;
      set
      {
        if(ShiftDirection != value)
        {
          _processor.ShiftDirection = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// ViewModel for providing shift values.
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
    /// The processor.
    /// </summary>
    private readonly BitShift _processor;

    #endregion Member

    #region Construction

    public BitshiftViewModel(BitShift processor)
      : base(processor)
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