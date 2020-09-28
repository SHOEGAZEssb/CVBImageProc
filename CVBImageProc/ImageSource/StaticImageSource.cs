using Stemmer.Cvb;
using System;

namespace CVBImageProc.ImageSource
{
  /// <summary>
  /// Image source providing a non-changing image.
  /// </summary>
  class StaticImageSource : IImageSource
  {
    #region Properties

    /// <summary>
    /// The image to provide.
    /// </summary>
    public Image CurrentImage { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="img">The image to provide.</param>
    public StaticImageSource(Image img)
    {
      CurrentImage = img ?? throw new ArgumentNullException(nameof(img));
    }

    #endregion Construction
  }
}
