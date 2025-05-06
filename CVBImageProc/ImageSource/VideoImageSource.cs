using Stemmer.Cvb;
using Stemmer.Cvb.Async;
using System;
using System.Threading.Tasks;

namespace CVBImageProc.ImageSource
{
  /// <summary>
  /// Image source that provides images of a video.
  /// </summary>
  internal sealed class VideoImageSource : IChangingImageSource
  {
    #region Properties

    /// <summary>
    /// The current image to provide.
    /// </summary>
    public Image CurrentImage => _device.DeviceImage;

    /// <summary>
    /// The grab state of the video.
    /// </summary>
    public bool Grab
    {
      get => _grab;
      set
      {
        if (Grab != value)
        {
          _grab = value;
          if (Grab)
            GrabImagesAsync().Forget();
        }
      }
    }
    private bool _grab;

    /// <summary>
    /// Event that is fired when the <see cref="CurrentImage"/> changed.
    /// </summary>
    public event EventHandler CurrentImageChanged;

    #endregion Properties

    #region Member

    /// <summary>
    /// The video device.
    /// </summary>
    private readonly Device _device;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="device">The video device.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="device"/> is null.</exception>
    public VideoImageSource(Device device)
    {
      _device = device ?? throw new ArgumentNullException(nameof(device));
    }

    #endregion Construction

    /// <summary>
    /// Snaps a single image.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task SnapAsync()
    {
      await _device.Stream.GetSnapshotAsync().ConfigureAwait(false);
      CurrentImageChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Grabs images.
    /// </summary>
    /// <returns>Task.</returns>
    private async Task GrabImagesAsync()
    {
      var stream = _device.Stream;
      try
      {
        stream.Start();
        while (Grab)
        {
          await stream.WaitAsync().ConfigureAwait(false);
          CurrentImageChanged?.Invoke(this, EventArgs.Empty);
        }
      }
      finally
      {
        stream.Abort();
        Grab = false;
      }
    }
  }
}