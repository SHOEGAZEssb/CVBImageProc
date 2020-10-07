using System.ComponentModel;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Interface for a ViewModel of a <see cref="CVBImageProcLib.Processing.PixelFilter.IPixelFilter"/>.
  /// </summary>
  interface IPixelFilterViewModel : INotifyPropertyChanged, IHasSettings
  {
    /// <summary>
    /// Name of the filter.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// If true, inverts the logic of the filter.
    /// </summary>
    bool Invert { get; set; }
  }
}