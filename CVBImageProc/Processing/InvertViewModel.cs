using CVBImageProc.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Invert"/> processor.
  /// </summary>
  class InvertViewModel : AOIPlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public PixelFilterChainViewModel PixelFilterChainVM { get; }

    #endregion Properties

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public InvertViewModel(Invert processor, bool isActive)
      : base(processor, isActive)
    {
      PixelFilterChainVM = new PixelFilterChainViewModel(processor);
      PixelFilterChainVM.SettingsChanged += SubVM_SettingsChanged;
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