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
    public string Name => _processor.Name;

    #endregion IProcessorViewModel Implementation

    #region Member

    /// <summary>
    /// The actual processor.
    /// </summary>
    private readonly IProcessor _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The actual processor.</param>
    public ProcessorViewModel(IProcessor processor)
    {
      _processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    #endregion Construction
  }
}