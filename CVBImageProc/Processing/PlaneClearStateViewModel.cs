namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for an individual plane clear state
  /// in the <see cref="PlaneClearViewModel"/>.
  /// </summary>
  internal class PlaneClearStateViewModel : PlaneSettingsViewModelBase
  {
    #region Properties

    /// <summary>
    /// Clear state of this plane.
    /// </summary>
    public bool Clear
    {
      get => _clear;
      set
      {
        if (Clear != value)
        {
          _clear = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }
    private bool _clear;

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="planeIndex">Index of the plane in the parent image.</param>
    public PlaneClearStateViewModel(int planeIndex)
      : base(planeIndex)
    { }

    #endregion Construction
  }
}