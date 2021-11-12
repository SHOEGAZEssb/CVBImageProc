using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Base class for pixel filter ViewModels.
  /// </summary>
  internal abstract class PixelFilterViewModelBase : SettingsViewModelBase, IPixelFilterViewModel
  {
    #region IPixelFilterViewModel Implementation

    /// <summary>
    /// The filter.
    /// </summary>
    public IPixelFilter Filter { get; }

    /// <summary>
    /// The name of the Filter.
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
    /// If true, the pixel filter gets processed.
    /// </summary>
    public bool IsActive
    {
      get => _isActive;
      set
      {
        if (IsActive != value)
        {
          _isActive = value;
          IsActiveChanged?.Invoke(this, EventArgs.Empty);
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }
    private bool _isActive;

    /// <summary>
    /// Event that is fired when <see cref="IsActive"/> changes.
    /// </summary>
    public event EventHandler IsActiveChanged;

    #endregion IPixelFilterViewModel Implementation

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="filter">The pixel filter.</param>
    /// <param name="isActive">Default active state.</param>
    protected PixelFilterViewModelBase(IPixelFilter filter, bool isActive)
    {
      Filter = filter ?? throw new ArgumentNullException(nameof(filter));
      _isActive = isActive;
    }

    #endregion Construction
  }
}