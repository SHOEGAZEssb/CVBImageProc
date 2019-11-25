using CVBImageProc.MVVM;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Base ViewModel for individual plane settings of a processor.
  /// </summary>
  abstract class PlaneSettingsViewModelBase : ViewModelBase, IHasSettings
  {
    #region IHasSettings Implementation

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    #endregion IHasSettings Implementation

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

    /// <summary>
    /// Fires the <see cref="SettingsChanged"/> event.
    /// </summary>
    protected void OnSettingsChanged()
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}