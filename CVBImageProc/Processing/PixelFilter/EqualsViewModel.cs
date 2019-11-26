using CVBImageProc.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.PixelFilter
{
  class EqualsViewModel : ViewModelBase, IPixelFilterViewModel
  {
    #region IPixelFilterViewModel Implementation

    public string Name => _filter.Name;

    public event EventHandler SettingsChanged;

    #endregion IPixelFilterViewModel Implementation

    #region Properties

    public byte CompareByte
    {
      get => _filter.CompareByte;
      set
      {
        if(CompareByte != value)
        {
          _filter.CompareByte = value;
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    #endregion Properties

    #region Member

    private readonly Equals _filter;

    #endregion Member

    #region Construction

    public EqualsViewModel(Equals filter)
    {
      _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    #endregion Construction
  }
}
