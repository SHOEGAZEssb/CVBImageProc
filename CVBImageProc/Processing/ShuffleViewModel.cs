namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Shuffle"/> processor.
  /// </summary>
  class ShuffleViewModel : PlaneProcessorViewModelBase
  {
    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    public ShuffleViewModel(Shuffle processor)
      : base(processor)
    { }

    #endregion Construction
  }
}