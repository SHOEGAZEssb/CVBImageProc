namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Sort"/> processor.
  /// </summary>
  class SortViewModel : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Mode to use while sorting.
    /// </summary>
    public SortMode Mode
    {
      get => _processor.Mode;
      set
      {
        if (Mode != value)
        {
          _processor.Mode = value;
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
    private readonly Sort _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    public SortViewModel(Sort processor)
      : base(processor)
    {
      _processor = processor;
    }

    #endregion Construction
  }
}