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
    /// The wrapped processor.
    /// </summary>
    IProcessor Processor { get; }
  }
}