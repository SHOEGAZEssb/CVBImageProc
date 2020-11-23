using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Pixelate"/> processor.
  /// </summary>
  class PixelateViewModel : FullProcessorViewModelBase
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
    }

    #endregion Construction
  }
}