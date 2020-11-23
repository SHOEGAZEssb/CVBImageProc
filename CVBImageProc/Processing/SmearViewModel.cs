using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Smear"/> processor.
  /// </summary>
  class SmearViewModel : FullProcessorViewModelBase
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

    public SmearViewModel(Smear processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
    }

    #endregion Construction
  }
}