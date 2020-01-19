using CVBImageProc.MVVM;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for a generic <see cref="IProcessor"/>.
  /// </summary>
  class ProcessorViewModel : ViewModelBase, IProcessorViewModel
  {
    #region IProcessorViewModel Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => Processor.Name;

    /// <summary>
    /// The wrapped processor.
    /// </summary>
    public IProcessor Processor { get; }

    #endregion IProcessorViewModel Implementation

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The actual processor.</param>
    public ProcessorViewModel(IProcessor processor)
    {
      Processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    #endregion Construction
  }
}