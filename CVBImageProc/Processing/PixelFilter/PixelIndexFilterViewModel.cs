using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for a <see cref="IPixelIndexFilter"/>.
  /// </summary>
  class PixelIndexFilterViewModel : PixelFilterViewModelBase
  {
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
    /// <param name="isActive">Active state of the filter.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
    public PixelIndexFilterViewModel(IPixelIndexFilter filter, bool isActive)
      : base(filter, isActive)
    {
      Filter = filter;
    }

    #endregion Construction
  }
}