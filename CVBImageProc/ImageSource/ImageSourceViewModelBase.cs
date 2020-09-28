using CVBImageProc.MVVM;
using Stemmer.Cvb;
using System;

namespace CVBImageProc.ImageSource
{
  public abstract class ImageSourceViewModelBase : ViewModelBase, IDisposable
  {
    #region Properties

    public Image CurrentImage => _imageSource.CurrentImage;

    #endregion Properties

    #region Member

    private readonly IImageSource _imageSource;

    #endregion Member

    #region Construction

    protected ImageSourceViewModelBase(IImageSource imageSource)
    {
      _imageSource = imageSource ?? throw new ArgumentNullException(nameof(imageSource));
    }

    #endregion Construction

    #region IDisposable Implementation

    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposedValue)
      {
        if (disposing)
        {
          if (_imageSource is IDisposable d)
            d.Dispose();
        }

        _disposedValue = true;
      }
    }

    ~ImageSourceViewModelBase()
    {
      Dispose(disposing: false);
    }

    public void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }

    #endregion IDisposable Implementation
  }
}
