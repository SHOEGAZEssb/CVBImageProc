using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Shuffle"/> processor.
  /// </summary>
  class ShuffleViewModel : FullProcessorViewModelBase
  {
    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public ShuffleViewModel(Shuffle processor, bool isActive)
      : base(processor, isActive)
    { }

    #endregion Construction
  }
}