using CVBImageProc.MVVM;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for a single plane of the <see cref="GainViewModel"/>.
  /// </summary>
  class PlaneGainViewModel : ViewModelBase, IHasSettings
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

    /// <summary>
    /// Value to add.
    /// </summary>
    public int Value
    {
      get => _value;
      set
      {
        if(Value != value)
        {
          if (value > MaxValue)
            value = MaxValue;
          else if (value < MinValue)
            value = MinValue;

          _value = value;
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }
    private int _value;

    /// <summary>
    /// Max value of the <see cref="Value"/>.
    /// </summary>
    public int MaxValue => 255;

    /// <summary>
    /// Min value of the <see cref="Value"/>.
    /// </summary>
    public int MinValue => -255;

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="planeIndex">Index of the plane.</param>
    public PlaneGainViewModel(int planeIndex)
    {
      PlaneIndex = planeIndex;
    }

    #endregion Construction
  }
}