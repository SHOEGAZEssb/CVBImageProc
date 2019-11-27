﻿using CVBImageProc.MVVM;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for a <see cref="IPixelFilter"/>.
  /// </summary>
  class PixelFilterViewModel : ViewModelBase, IPixelFilterViewModel
  {
    #region IPixelFilterViewModel Implementation

    /// <summary>
    /// Name of the filter.
    /// </summary>
    public string Name => _filter.Name;

    /// <summary>
    /// Byte to compare to.
    /// </summary>
    public byte CompareByte
    {
      get => _filter.CompareByte;
      set
      {
        if(CompareByte != value)
        {
          _filter.CompareByte = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// If true, inverts the logic of the filter.
    /// </summary>
    public bool Not
    {
      get => _filter.Not;
      set
      {
        if (Not != value)
        {
          _filter.Not = value;
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

    #region Member

    /// <summary>
    /// The filter.
    /// </summary>
    private readonly IPixelFilter _filter;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="filter">The filter.</param>
    public PixelFilterViewModel(IPixelFilter filter)
    {
      _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    #endregion Construction
  }
}