using Stemmer.Cvb;

namespace CVBImageProc.ImageSource
{
  /// <summary>
  /// Interface for a source providing images to process.
  /// </summary>
  public interface IImageSource
  {
    #region Properties

    /// <summary>
    /// The current image to provide.
    /// </summary>
    Image CurrentImage { get; }

    #endregion Properties
  }
}