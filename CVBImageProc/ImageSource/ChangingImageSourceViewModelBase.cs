using System;

namespace CVBImageProc.ImageSource
{
  /// <summary>
  /// Base class for <see cref="IChangingImageSource"/> ViewModels.
  /// </summary>
  public abstract class ChangingImageSourceViewModelBase : ImageSourceViewModelBase
  {
    #region Properties

    /// <summary>
    /// Event that is fired when the <see cref="IImageSource.CurrentImage"/> changed.
    /// </summary>
    public event EventHandler CurrentImageChanged;

    #endregion Properties

    #region Member

    /// <summary>
    /// The image source.
    /// </summary>
    private readonly IChangingImageSource _changingImageSource;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="imageSource">The image source.</param>
    protected ChangingImageSourceViewModelBase(IChangingImageSource imageSource)
      : base(imageSource)
    {
      _changingImageSource = imageSource;
      _changingImageSource.CurrentImageChanged += ImageSource_CurrentImageChanged;
    }

    #endregion Construction

    /// <summary>
    /// Disposes this object.
    /// </summary>
    /// <param name="disposing">True if called by the user.</param>
    protected override void Dispose(bool disposing)
    {
      _changingImageSource.CurrentImageChanged -= ImageSource_CurrentImageChanged;
      base.Dispose(disposing);
    }

    /// <summary>
    /// Fires the <see cref="CurrentImageChanged"/> event.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void ImageSource_CurrentImageChanged(object sender, EventArgs e)
    {
      CurrentImageChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}
