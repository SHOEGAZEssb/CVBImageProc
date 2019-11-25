using CVBImageProc.MVVM;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for a single plane of the <see cref="GainViewModel"/>.
  /// </summary>
  class PlaneGainViewModel : PlaneSettingsViewModelBase
  {
    #region Properties

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
          OnSettingsChanged();
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
      : base(planeIndex)
    { }

    #endregion Construction
  }
}