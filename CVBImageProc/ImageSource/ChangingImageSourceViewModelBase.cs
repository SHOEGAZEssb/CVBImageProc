using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.ImageSource
{
  public abstract class ChangingImageSourceViewModelBase : ImageSourceViewModelBase
  {
    #region Properties

    public event EventHandler CurrentImageChanged;

    #endregion Properties

    #region Member

    private readonly IChangingImageSource _changingImageSource;

    #endregion Member

    #region Construction

    protected ChangingImageSourceViewModelBase(IChangingImageSource imageSource)
      : base(imageSource)
    {
      _changingImageSource = imageSource;
      _changingImageSource.CurrentImageChanged += ImageSource_CurrentImageChanged;
    }

    #endregion Construction

    protected override void Dispose(bool disposing)
    {
      _changingImageSource.CurrentImageChanged -= ImageSource_CurrentImageChanged;
      base.Dispose(disposing);
    }

    private void ImageSource_CurrentImageChanged(object sender, EventArgs e)
    {
      CurrentImageChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}
