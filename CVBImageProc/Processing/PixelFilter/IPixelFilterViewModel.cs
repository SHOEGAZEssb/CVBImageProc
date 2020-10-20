using CVBImageProcLib.Processing.PixelFilter;
using System;
using System.ComponentModel;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Interface for a ViewModel of a <see cref="CVBImageProcLib.Processing.PixelFilter.IPixelFilter"/>.
  /// </summary>
  interface IPixelFilterViewModel : INotifyPropertyChanged, IHasSettings
  {
    /// <summary>
    /// The filter.
    /// </summary>
    IPixelFilter Filter { get; }

    /// <summary>
    /// Name of the filter.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// If true, inverts the logic of the filter.
    /// </summary>
    bool Invert { get; set; }

    /// <summary>
    /// Indicates if this pixel filter is
    /// active in the pixel filter chain.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Event that is fired when the
    /// <see cref="IsActive"/> changed.
    /// </summary>
    event EventHandler IsActiveChanged;
  }
}