namespace CVBImageProc.Processing
{
  /// <summary>
  /// Base ViewModel for individual plane settings of a processor.
  /// </summary>
  abstract class PlaneSettingsViewModelBase : SettingsViewModelBase
  {
    #region Properties

    /// <summary>
    /// Index of the plane in the image.
    /// </summary>
    public int PlaneIndex { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="planeIndex">Index of the plane in the image.</param>
    protected PlaneSettingsViewModelBase(int planeIndex)
    {
      PlaneIndex = planeIndex;
    }

    #endregion Construction
  }
}