using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for the <see cref="Randomize"/> pixel filter.
  /// </summary>
  class RandomizeViewModel : PixelFilterViewModelBase, IPixelAutoFilterViewModel
  {
    #region Properties

    /// <summary>
    /// Chance that the check passes.
    /// </summary>
    public double Chance
    {
      get => _filter.Chance;
      set
      {
        if (Chance != value)
        {
          _filter.Chance = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    #endregion Properties

    #region Member

    /// <summary>
    /// The filter.
    /// </summary>
    private readonly Randomize _filter;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="isActive">Active state of the filter.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
    public RandomizeViewModel(Randomize filter, bool isActive)
      : base(filter, isActive)
    {
      _filter = filter;
    }

    #endregion Construction
  }
}