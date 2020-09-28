using CVBImageProc.MVVM;
using CVBImageProc.Processing.ValueProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CVBImageProc.ImageSource
{
  class VideoImageSourceViewModel : ChangingImageSourceViewModelBase
  {
    #region Properties

    public bool Grab
    {
      get => _videoImageSource.Grab;
      set
      {
        if(Grab != value)
        {
          _videoImageSource.Grab = value;
          NotifyOfPropertyChange();
        }
      }
    }

    public ICommand ToggleGrabCommand { get; }

    public ICommand SnapCommand { get; }

    #endregion Properties

    #region Member

    private readonly VideoImageSource _videoImageSource;

    #endregion Member

    #region Construction

    public VideoImageSourceViewModel(VideoImageSource imageSource)
      : base(imageSource)
    {
      _videoImageSource = imageSource;
      ToggleGrabCommand = new DelegateCommand((o) => ToggleGrab());
      SnapCommand = new DelegateCommand(async (o) => await Snap().ConfigureAwait(false));
    }

    #endregion Construction

    private void ToggleGrab()
    {
      Grab = !Grab;
    }

    private async Task Snap()
    {
      await _videoImageSource.Snap().ConfigureAwait(false);
    }
  }
}
