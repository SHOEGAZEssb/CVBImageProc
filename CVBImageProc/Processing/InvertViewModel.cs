using CVBImageProc.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Invert"/> processor.
  /// </summary>
  class InvertViewModel : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public IndividualPixelProcessorSettingsViewModel PixelFilterChainVM { get; }

    #endregion Properties

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    public InvertViewModel(Invert processor)
      : base(processor)
    {
      PixelFilterChainVM = new IndividualPixelProcessorSettingsViewModel(processor);
      PixelFilterChainVM.SettingsChanged += PixelFilterChainVM_SettingsChanged;
    }

    /// <summary>
    /// Fires the SettingsChanged event when the
    /// pixel filter settings changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void PixelFilterChainVM_SettingsChanged(object sender, EventArgs e)
    {
      OnSettingsChanged();
    }
  }
}