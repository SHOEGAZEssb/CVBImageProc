using System.ComponentModel;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Interface for a ViewModel of a <see cref="IPixelFilter"/>.
  /// </summary>
  interface IPixelFilterViewModel : INotifyPropertyChanged, IHasSettings
  {
    /// <summary>
    /// Name of the filter.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Byte to compare to.
    /// </summary>
    byte CompareByte { get; set; }

    /// <summary>
    /// If true, inverts the logic of the filter.
    /// </summary>
    bool Not { get; set; }
  }
}