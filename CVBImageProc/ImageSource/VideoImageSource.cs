using Stemmer.Cvb;
using Stemmer.Cvb.Async;
using System;
using System.Threading.Tasks;

namespace CVBImageProc.ImageSource
{
  class VideoImageSource : IChangingImageSource
  {
    #region Properties

    public Image CurrentImage => _device.DeviceImage;

    public event EventHandler CurrentImageChanged;

    #endregion Properties

    #region Member

    private readonly Device _device;

    #endregion Member

    #region Construction

    public VideoImageSource(Device device)
    {
      _device = device ?? throw new ArgumentNullException(nameof(device));
    }

    #endregion Construction

    public async Task Snap()
    {
      await _device.Stream.GetSnapshotAsync().ConfigureAwait(false);
      CurrentImageChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}
