using System;
using System.ComponentModel;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Interface for a ViewModel managing a <see cref="IProcessor"/>.
  /// </summary>
  public interface IProcessorViewModel : INotifyPropertyChanged
  {
    /// <summary>
    /// Name of the processor
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Indicates if this processor is
    /// active in the processor chain.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Event that is fired when the
    /// <see cref="IsActive"/> changed.
    /// </summary>
    event EventHandler IsActiveChanged;

    /// <summary>
    /// The wrapped processor.
    /// </summary>
    IProcessor Processor { get; }
  }
}