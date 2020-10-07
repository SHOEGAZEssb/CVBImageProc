using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for a <see cref="IPixelValueFilter"/>.
  /// </summary>
  class PixelValueFilterViewModel : ViewModelBase, IPixelFilterViewModel
  {
    #region IPixelFilterViewModel Implementation

    /// <summary>
    /// The filter.
    /// </summary>
    public IPixelValueFilter Filter { get; }

    /// <summary>
    /// Name of the filter.
    /// </summary>
    public string Name => Filter.Name;

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
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    /// <summary>
    /// Fires the <see cref="SettingsChanged"/> event.
    /// </summary>
    protected void OnSettingsChanged()
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion IPixelFilterViewModel Implementation

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
    public PixelValueFilterViewModel(IPixelValueFilter filter)
    {
      Filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    #endregion Construction
  }
}