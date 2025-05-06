using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for a <see cref="IPixelIndexFilter"/>.
  /// </summary>
  internal sealed class PixelIndexFilterViewModel : PixelFilterViewModelBase
  {
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
    /// <param name="isActive">Active state of the filter.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
    public PixelIndexFilterViewModel(IPixelIndexFilter filter, bool isActive)
      : base(filter, isActive)
    {
      _filter = filter;
    }

    #endregion Construction
  }
}