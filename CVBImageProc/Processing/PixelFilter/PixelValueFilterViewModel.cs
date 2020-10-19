using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for a <see cref="IPixelValueFilter"/>.
  /// </summary>
  class PixelValueFilterViewModel : PixelFilterViewModelBase
  {
    #region Properties

    /// <summary>
    /// The filter.
    /// </summary>
    public IPixelValueFilter Filter { get; }

    /// <summary>
    /// Byte to compare to.
    /// </summary>
    public byte CompareByte
    {
      get => Filter.CompareByte;
      set
      {
        if (CompareByte != value)
        {
          Filter.CompareByte = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// Max value of the <see cref="CompareByte"/>.
    /// </summary>
    public byte MaxCompareByte => Filter.MaxCompareByte;

    /// <summary>
    /// Min value of the <see cref="CompareByte"/>.
    /// </summary>
    public byte MinCompareByte => Filter.MinCompareByte;

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="isActive">Active state of the filter.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
    public PixelValueFilterViewModel(IPixelValueFilter filter, bool isActive)
      : base(filter, isActive)
    {
      Filter = filter;
    }

    #endregion Construction
  }
}