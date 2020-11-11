using CVBImageProc.Processing.PixelFilter;
using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Pixelate"/> processor.
  /// </summary>
  class PixelateViewModel : AOIPlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Size of each new "pixel".
    /// </summary>
    public int PixelateSize
    {
      get => _processor.PixelateSize;
      set
      {
        if(PixelateSize != value)
        {
          _processor.PixelateSize = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public PixelFilterChainViewModel PixelFilterChainVM { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly Pixelate _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Default active state.</param>
    public PixelateViewModel(Pixelate processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
      PixelFilterChainVM = new PixelFilterChainViewModel(_processor);
      PixelFilterChainVM.SettingsChanged += SubVM_SettingsChanged;
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