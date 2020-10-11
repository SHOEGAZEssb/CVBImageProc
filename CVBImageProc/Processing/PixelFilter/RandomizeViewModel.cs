using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for the <see cref="Randomize"/> pixel filter.
  /// </summary>
  class RandomizeViewModel : ViewModelBase, IPixelAutoFilterViewModel
  {
    #region Properties

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    /// <summary>
    /// The filter.
    /// </summary>
    public IPixelAutoFilter Filter => _filter;
    private readonly Randomize _filter;

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
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

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
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
    public RandomizeViewModel(Randomize filter)
    {
      _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    #endregion Construction
  }
}