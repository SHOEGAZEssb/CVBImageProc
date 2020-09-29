namespace CVBImageProc.ImageSource
{
  /// <summary>
  /// ViewModel for a <see cref="StaticImageSource"/>.
  /// </summary>
  class StaticImageSourceViewModel : ImageSourceViewModelBase
  {
    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="imageSource">The image source.</param>
    public StaticImageSourceViewModel(StaticImageSource imageSource)
      : base(imageSource)
    { }

    #endregion Construction
  }
}
