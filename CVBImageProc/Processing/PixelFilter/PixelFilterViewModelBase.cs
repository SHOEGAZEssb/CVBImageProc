using CVBImageProcLib.Processing.PixelFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.PixelFilter
{
  abstract class PixelFilterViewModelBase : SettingsViewModelBase, IPixelFilterViewModel
  {
    #region IPixelFilterViewModel Implementation

    public string Name => _filter.Name;

    /// <summary>
    /// If true, inverts the logic of the filter.
    /// </summary>
    public bool Invert
    {
      get => _filter.Invert;
      set
      {
        if (Invert != value)
        {
          _filter.Invert = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    public bool IsActive
    {
      get => _isActive;
      set
      {
        if(IsActive != value)
        {
          _isActive = value;
          IsActiveChanged?.Invoke(this, EventArgs.Empty);
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }
    private bool _isActive;

    public event EventHandler IsActiveChanged;

    #endregion IPixelFilterViewModel Implementation

    #region Member

    private readonly IPixelFilter _filter;

    #endregion Member

    #region Construction

    protected PixelFilterViewModelBase(IPixelFilter filter, bool isActive)
    {
      _filter = filter ?? throw new ArgumentNullException(nameof(filter));
      _isActive = isActive;
    }

    #endregion Construction
  }
}