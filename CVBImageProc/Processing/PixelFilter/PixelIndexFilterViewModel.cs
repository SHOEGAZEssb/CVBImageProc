using CVBImageProc.MVVM;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for a <see cref="IPixelIndexFilter"/>.
  /// </summary>
  class PixelIndexFilterViewModel : ViewModelBase, IPixelFilterViewModel
  {
    #region IPixelFilterViewModel Implementation

    /// <summary>
    /// Name of the filter.
    /// </summary>
    public string Name => _filter.Name;

    /// <summary>
    /// If true, inverts the logic of the filter.
    /// </summary>
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

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    private void OnSettingsChanged()
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion IPixelFilterViewModel Implementation

    #region Properties

    /// <summary>
    /// Index value to compare to.
    /// </summary>
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

    /// <summary>
    /// Minimum index value to compare to.
    /// </summary>
    public int MinCompareValue => _filter.MinCompareValue;

    #endregion Properties

    #region Member

    /// <summary>
    /// The filter.
    /// </summary>
    private readonly IPixelIndexFilter _filter;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
    public PixelIndexFilterViewModel(IPixelIndexFilter filter)
    {
      _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    #endregion Construction
  }
}