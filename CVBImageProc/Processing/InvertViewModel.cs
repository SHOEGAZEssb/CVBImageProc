using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Invert"/> processor.
  /// </summary>
  internal sealed class InvertViewModel : FullProcessorViewModelBase
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public InvertViewModel(Invert processor, bool isActive)
      : base(processor, isActive)
    { }
  }
}