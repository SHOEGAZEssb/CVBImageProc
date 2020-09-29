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
    public string Name => Filter.Name;

    /// <summary>
    /// If true, inverts the logic of the filter.
    /// </summary>
    public bool Invert
    {
      get => Filter.Invert;
      set
      {
        if (Invert != value)
        {
          Filter.Invert = value;
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
    /// The filter.
    /// </summary>
    public IPixelIndexFilter Filter { get; }

    /// <summary>
    /// Index value to compare to.
    /// </summary>
    public int CompareValue
    {
      get => Filter.CompareValue;
      set
      {
        if (CompareValue != value)
        {
          Filter.CompareValue = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Minimum index value to compare to.
    /// </summary>
    public int MinCompareValue => Filter.MinCompareValue;

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
    public PixelIndexFilterViewModel(IPixelIndexFilter filter)
    {
      Filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    #endregion Construction
  }
}