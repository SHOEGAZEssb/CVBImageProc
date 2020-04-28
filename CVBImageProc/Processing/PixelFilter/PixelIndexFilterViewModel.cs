using CVBImageProc.MVVM;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  class PixelIndexFilterViewModel : ViewModelBase, IPixelFilterViewModel
  {
    #region IPixelFilterViewModel Implementation

    public string Name => _filter.Name;

    public bool Invert
    {
      get => _filter.Invert;
      set
      {
        if(Invert != value)
        {
          _filter.Invert = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    public event EventHandler SettingsChanged;

    private void OnSettingsChanged()
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion IPixelFilterViewModel Implementation

    #region Properties

    public int CompareValue
    {
      get => _filter.CompareValue;
      set
      {
        if (CompareValue != value)
        {
          _filter.CompareValue = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    public int MinCompareValue => _filter.MinCompareValue;

    #endregion Properties

    #region Member

    private readonly IPixelIndexFilter _filter;

    #endregion Member

    #region Construction

    public PixelIndexFilterViewModel(IPixelIndexFilter filter)
    {
      _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    #endregion Construction
  }
}
