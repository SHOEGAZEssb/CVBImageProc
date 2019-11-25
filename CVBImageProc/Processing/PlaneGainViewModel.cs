using CVBImageProc.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing
{
  class PlaneGainViewModel : ViewModelBase, IHasSettings
  {
    #region IHasSettings Implementation

    public event EventHandler SettingsChanged;

    #endregion IHasSettings Implementation

    #region Properties

    public int PlaneIndex { get; }

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

    public int MaxValue => 255;
    public int MinValue => -255;

    #endregion Properties

    #region Construction

    public PlaneGainViewModel(int planeIndex)
    {
      PlaneIndex = planeIndex;
    }

    #endregion Construction
  }
}
