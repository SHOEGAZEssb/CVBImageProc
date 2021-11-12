using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Smear"/> processor.
  /// </summary>
  internal class SmearViewModel : FullProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Direction to smear pixels in.
    /// </summary>
    public SmearMode Mode
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

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly Smear _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Default active state.</param>
    public SmearViewModel(Smear processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
    }

    #endregion Construction
  }
}